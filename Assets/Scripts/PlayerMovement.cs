using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    enum Lanes
    {
        Left, //0
        Center, //1
        Right //2
    }

    //Unity Events: Please use these to handle other logic when these events occur.
    //Please don't hook into Update or other functions in this script for these events.
    //Oh and remember to assign functions in inspector/through script :)
    public UnityEvent OnStumble; //Called when the player hits into a wall but does not die
    public UnityEvent OnRecover; //Called when the player has recovered from a stumble and is back to normal
    public UnityEvent OnLaneChange; //Called when the player successfully changes lanes
    public UnityEvent OnJump; //Called when the player is mid air
    public UnityEvent OnGameOver; //Called when the player dies. RIP.
    public UnityEvent OnDash; //Called when the player dashes forward
    public UnityEvent OnDashFinish; //Called when the player's dash finishes

    //Input System
    private InputAction moveAction;
    private InputAction jumpAction;
    private float moveInput;
    private InputAction dashAction;
    private GameMaster gameMaster;
    private LevelSpawner levelSpawner;
    public PlayerDashAndDisplay dashAndDisplay;

    [Header("Movement Speed and Input Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckRayCastDistance;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float laneWidth = 2f;
    [SerializeField] private float nextInputDelay = 3f; //Time delay between lane switch inputs
    [SerializeField] private float jumpInputDelay = 1.0f;
    [SerializeField] private float stumbleInvincibilityTime;
    [SerializeField] private float stumbleRecoverTime;
    [SerializeField] private bool isPlayerDashing = false;

    private float currentJumpDelay;
    private float currentStumbleInvincibilityTime;

    private float inputDelayTimer = 0f;
    private Lanes currentLane = Lanes.Center;
    private Rigidbody playerRigidbody;

    //Stumble Values
    [Header("Stumble Settings")]
    private bool isStumbling = false;
    [SerializeField] private Transform stumbleCheckOrigin;
    [SerializeField] private float stumbleRayDefaultDistance;

    [Tooltip("How much the stumble check ray's distance is multiplied by world speed e.g. 1 is multiply world speed x default distance and 2 is 2 x world speed x default distance")]
    [SerializeField] private float worldSpeedRayDistanceMultiplier = 1f;
    [SerializeField] LayerMask obstacleLayers;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameMaster = GameObject.Find("Game Master").GetComponent<GameMaster>();
        levelSpawner = GameObject.Find("Level Spawner").GetComponent<LevelSpawner>();

        InitialiseControlScheme();
        jumpAction = InputSystem.actions.FindAction("Jump");

        playerRigidbody = GetComponent<Rigidbody>();

        //Adding a listener to the OnStumble event through script
        OnStumble.AddListener(StumbleHandle);
        if(stumbleRecoverTime == stumbleInvincibilityTime)
        {
            Debug.LogError("Player cannot die as stumble recover time and stumble invincibility time are the same in PlayerMovement");
        }
        else if(stumbleInvincibilityTime > stumbleRecoverTime)
        {
            Debug.LogError("Player cannot die as stumble recover time is lower than stumble invincibility time in PlayerMovement");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Process no player movement if the game has not started (aka still in a menu)
        if(gameMaster.GetGameplayState() == false) { return; }

        if(inputDelayTimer <= 0f)
        {
            moveInput = moveAction.ReadValue<float>();
            if(moveInput != 0){Debug.Log(moveInput); }
            //Handle lane switching
            if(moveInput < 0) //Move Left
            {
                if(currentLane == Lanes.Center)
                {
                    SwitchLane(Lanes.Left);
                }
                else if(currentLane == Lanes.Right)
                {
                    SwitchLane(Lanes.Center);
                }
                else
                {
                    //Stumble eventually
                    //Debug.Log("Already in Left Lane");
                    OnStumble.Invoke();
                }
                inputDelayTimer = nextInputDelay;
            }
            else if(moveInput > 0) //Move Right
            {
                if(currentLane == Lanes.Center)
                {
                    SwitchLane(Lanes.Right);
                }
                else if(currentLane == Lanes.Left)
                {
                    SwitchLane(Lanes.Center);
                }
                else
                {
                    //Stumble eventually
                    //Debug.Log("Already in Right Lane");
                    OnStumble.Invoke();
                }
                inputDelayTimer = nextInputDelay;
            }
        }
        else
        {
            inputDelayTimer -= Time.deltaTime;
        }
        //Handle Jump
        if (jumpAction.WasPressedThisFrame() && currentJumpDelay <= 0f && GroundCheck())
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            OnJump.Invoke();
            currentJumpDelay = jumpInputDelay;
        }
        else
        {
            currentJumpDelay -= Time.deltaTime;
        }
        //Stumble invinicibility time tick down
        if(currentStumbleInvincibilityTime > 0)
        {
            currentStumbleInvincibilityTime -= Time.deltaTime;
        }
        if(dashAction.WasPressedThisFrame() && dashAndDisplay.canDash)
        {
            OnPlayerDash();
            OnDash.Invoke();
        }
    }

    private bool GroundCheck()
    {
        RaycastHit groundHit;
        //Raycast down from stumble check origin which is slightly in front of player
        if(Physics.Raycast(groundCheckTransform.position, Vector3.down, out groundHit, groundCheckRayCastDistance, groundLayers))
        {
            Debug.DrawLine(groundCheckTransform.position, groundHit.point, Color.green, 2);
            //Debug.Log("Grounded.");
            return true;
        }
        Vector3 endOfRay = groundCheckTransform.position + (Vector3.down * groundCheckRayCastDistance);
        Debug.DrawLine(groundCheckTransform.position, endOfRay, Color.red, 2);
        return false;
    }

    public void InitialiseControlScheme()
    {
        if (PlayerPrefs.HasKey("ControlSchemeKey"))
        {
            //change the referenced input actions based on current control scheme pulled from player prefs
            switch (PlayerPrefs.GetInt("ControlSchemeKey"))
            {
                case 0:
                    moveAction = InputSystem.actions.FindAction("Move (WASD)");
                    dashAction = InputSystem.actions.FindAction("Dash (WASD)");
                    break;
                case 1:
                    moveAction = InputSystem.actions.FindAction("Move (ArrowKeys)");
                    dashAction = InputSystem.actions.FindAction("Dash (ArrowKeys)");
                    break;
                default:
                    moveAction = InputSystem.actions.FindAction("Move (WASD)");
                    dashAction = InputSystem.actions.FindAction("Dash (WASD)");
                    break;
            }
        }
        else
        {
            moveAction = InputSystem.actions.FindAction("Move (WASD)");
            dashAction = InputSystem.actions.FindAction("Dash (WASD)");
        }
    }

    private void SwitchLane(Lanes targetLane)
    {
        float targetX = 0f;
        switch(targetLane)
        {
            case Lanes.Left:
                targetX = -laneWidth;
                break;
            case Lanes.Center:
                targetX = 0f;
                break;
            case Lanes.Right:
                targetX = laneWidth;
                break;
        }

        //Do a raycast every time we switch lane just before the switch takes place to see if it was a close call or not
        //If it was a close call, we'll stumble but let the switch occur.
        RaycastHit hit;
        float raycastDistance = worldSpeedRayDistanceMultiplier * (levelSpawner.GetSpeed() / 10) * stumbleRayDefaultDistance;
        //Debug.Log("Raycast Distance: " + raycastDistance);
        if(Physics.Raycast(stumbleCheckOrigin.position, stumbleCheckOrigin.forward, out hit, raycastDistance, obstacleLayers))
        {
            Debug.Log("Stumbled from close call");
            Debug.DrawLine(stumbleCheckOrigin.position, hit.point, Color.red, 2);
            StumbleHandle();
            StartCoroutine(LaneSwitch(targetX));
            currentLane = targetLane;
            OnLaneChange.Invoke();
        }
        else
        {
            //Debug.Log("Smooth lane switch!");
            Debug.DrawLine(stumbleCheckOrigin.position, stumbleCheckOrigin.position + stumbleCheckOrigin.forward * raycastDistance, Color.green, 2);
            StartCoroutine(LaneSwitch(targetX));    
            //Invoke unity event when the lane switch is complete
            currentLane = targetLane;
            OnLaneChange.Invoke();
        }
    }

    private IEnumerator LaneSwitch(float targetX)
    {
        //Math to make player smoothly switch lanes instead of snapping between them
        float switchDuration = 0.2f;
        float elapsedTime = 0f;
        float startX = playerRigidbody.position.x;
        while(elapsedTime < switchDuration)
        {
            elapsedTime += Time.deltaTime;
            float newX = Mathf.SmoothStep(startX, targetX, elapsedTime / switchDuration);
            Vector3 newPosition = new Vector3(newX, playerRigidbody.position.y, playerRigidbody.position.z);
            playerRigidbody.MovePosition(newPosition);
            yield return null;  
        }
        Vector3 newPos = new Vector3(targetX, playerRigidbody.position.y, playerRigidbody.position.z);
        playerRigidbody.MovePosition(newPos);
        OnLaneChange.Invoke();
    }

    //This function gets called when OnStumble event is invoked
    private void StumbleHandle()
    {
        Debug.Log("I frame duration: " + stumbleInvincibilityTime + " Current I frame duration: " + currentStumbleInvincibilityTime);
        //If stumbling during I frames, return and do nothing
        if(currentStumbleInvincibilityTime > 0) { return; }
        //If not already stumbling and current invincibilty time <= 0, stumble
        else if(!isStumbling && currentStumbleInvincibilityTime <= 0)
        {
            Debug.Log("First stumble");
            isStumbling = true;
            currentStumbleInvincibilityTime = stumbleInvincibilityTime;
            OnStumble.Invoke();
            Invoke("RecoverFromStumble", stumbleRecoverTime);
        }
        //If stumbling while already stumbling outside of I frames, game over
        else if(isStumbling && currentStumbleInvincibilityTime <= 0)
        {
            GameOverHandle();
        }
        else
        {
            Debug.LogError("StumbleHandle reached unintended branch on line 240 in PlayerMovement.cs");
        }
    }
    public void OnPlayerDash()
    {
        isPlayerDashing = true; //toggle to let player destroy obstacles instead of dying to them
        dashAndDisplay.OnPlayerDash(); //reset dash meter
        Debug.Log("Dash started at " + Time.time);
        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        float startingDashSpeed = levelSpawner.GetSpeed();
        float maxDashSpeed = levelSpawner.GetSpeed() * 3;
        //bool isDashIncreasing = true;
        bool hasDashFinished = false;

        //increase player's speed
        for (float increasingSpeed = levelSpawner.GetSpeed(); levelSpawner.GetSpeed() < maxDashSpeed; increasingSpeed += (startingDashSpeed / 10))
        {
            levelSpawner.SetSpeed(increasingSpeed);
            yield return new WaitForSeconds(0.02f);
        }

        //duration player will stay at max speed of dash before decreasing
        yield return new WaitForSeconds(2f);

        //decrease player's speed
        for (float decreasingSpeed = levelSpawner.GetSpeed(); levelSpawner.GetSpeed() > startingDashSpeed; decreasingSpeed -= ((startingDashSpeed * 3) / 20))
        {
            levelSpawner.SetSpeed(decreasingSpeed);
            yield return new WaitForSeconds(0.2f);

            if(levelSpawner.GetSpeed() <= startingDashSpeed)
            {
                hasDashFinished = true;
                levelSpawner.SetSpeed(startingDashSpeed);
                break;
            }
        }
        
        if (hasDashFinished)
        {
            isPlayerDashing = false;
            OnDashFinish.Invoke();
            Debug.Log("Dash finished at " + Time.time);
            yield return null;
        }
    }

    private void GameOverHandle()
    {
        OnGameOver.Invoke();
        Debug.Log("Game Over");
    }

    private void RecoverFromStumble()
    {
        isStumbling = false;
        OnRecover.Invoke();
    }

    public bool GetIsPlayerDashing()
    {
        return isPlayerDashing;
    }
}
