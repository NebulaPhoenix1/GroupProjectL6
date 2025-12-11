using System.Collections;
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

    //Input System
    private InputAction moveAction;
    private InputAction jumpAction;
    private float moveInput;
    private GameMaster gameMaster;
    private LevelSpawner levelSpawner;

    [Header("Movement Speed and Input Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float laneWidth = 1.5f;
    [SerializeField] private float nextInputDelay = 3f; //Time delay between lane switch inputs
    [SerializeField] private float jumpInputDelay = 1.0f;
    private float currentJumpDelay;

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
    [SerializeField] LayerMask stumbleRayCastExcludedlayers;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameMaster = GameObject.Find("Game Master").GetComponent<GameMaster>();
        levelSpawner = GameObject.Find("Level Spawner").GetComponent<LevelSpawner>();

        playerRigidbody = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        //Adding a listener to the OnStumble event through script
        OnStumble.AddListener(StumbleHandle);
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
                    Debug.Log("Already in Left Lane");
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
                    Debug.Log("Already in Right Lane");
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
        if (jumpAction.WasPressedThisFrame() && currentJumpDelay <= 0f)
        {
            playerRigidbody.AddForce(new Vector3(0, jumpForce, 0));
            currentJumpDelay = jumpInputDelay;
        }
        else
        {
            currentJumpDelay -= Time.deltaTime;
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
        float raycastDistance = worldSpeedRayDistanceMultiplier * levelSpawner.GetSpeed() * stumbleRayDefaultDistance;
        Debug.Log("Raycast Distance: " + raycastDistance);
        if(Physics.Raycast(stumbleCheckOrigin.position, stumbleCheckOrigin.forward, out hit, raycastDistance, stumbleRayCastExcludedlayers))
        {
            Debug.Log("Stumbled..");
            Debug.DrawLine(stumbleCheckOrigin.position, hit.point, Color.red, 2);
            StumbleHandle();
            StartCoroutine(LaneSwitch(targetX));
            currentLane = targetLane;
            OnLaneChange.Invoke();
        }
        else
        {
            Debug.Log("Smooth lane switch!");
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
            playerRigidbody.position = new Vector3(newX, playerRigidbody.position.y, playerRigidbody.position.z);
            yield return null;  
        }
        playerRigidbody.position = new Vector3(targetX, playerRigidbody.position.y, playerRigidbody.position.z);
        OnLaneChange.Invoke();
    }

    //This function gets called when OnStumble event is invoked
    private void StumbleHandle()
    {
        //We need to check if the player is already stumbling; if so invoke OnGameOver
        if(isStumbling)
        {
            OnGameOver.Invoke();
            Debug.Log("Game Over! Player has stumbled again while already stumbling.");
        }
        else
        {
            isStumbling = true;
            
            OnStumble.Invoke();
            //Recover after 1 second
            Invoke("RecoverFromStumble", 1.0f);
        }
    }

    private void RecoverFromStumble()
    {
        isStumbling = false;
        OnRecover.Invoke();
    }
}
