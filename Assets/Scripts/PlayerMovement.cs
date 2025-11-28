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
    public UnityEvent OnGameOver; //Called when the player dies. RIP.

    //Input System
    private InputAction moveAction;
    private float moveInput;

    
    [Header("Movement Speed and Input Settings")]
    [SerializeField] private float movementSpeed = 2.0f;
    [SerializeField] private float movementSpeedGainPerSecond = 0.1f;
    [SerializeField] private float maxMovementSpeed = 5.0f;
    [SerializeField] private float laneWidth = 1.5f;
    [SerializeField] private float nextInputDelay = 0.2f; //Time delay between lane switch inputs
    private float inputDelayTimer = 0f;
    private float speedGainCooldown;
    private Lanes currentLane = Lanes.Center;
    private Rigidbody playerRigidbody;

    //Stumble Values
    private bool isStumbling = false; 
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        //Adding a listener to the OnStumble event through script
        OnStumble.AddListener(StumbleHandle);
    }

    // Update is called once per frame
    void Update()
    {
        if(movementSpeed < maxMovementSpeed)
        {
            speedGainCooldown+=Time.deltaTime;
            if(speedGainCooldown >= 1.0f)
            {
                movementSpeed += movementSpeedGainPerSecond;
                speedGainCooldown = 0f;
            }
        }
        playerRigidbody.linearVelocity = new Vector3(0, 0, movementSpeed);
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
        playerRigidbody.position = new Vector3(targetX, playerRigidbody.position.y, playerRigidbody.position.z);
        currentLane = targetLane;
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
            //Reduce speed temporarily
            movementSpeed *= 0.5f;
            //Recover after 1 second
            Invoke("RecoverFromStumble", 1.0f);
        }
    }

    private void RecoverFromStumble()
    {
        isStumbling = false;
        //Restore speed
        movementSpeed *= 2.0f;
        OnRecover.Invoke();
    }
}
