using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    enum Lanes
    {
        Left, //0
        Center, //1
        Right //2
    }
    
    //[SerializeField] private InputAction moveAction;
    [SerializeField] private float movementSpeed = 2.0f;
    [SerializeField] private float movementSpeedGainPerSecond = 0.1f;
    [SerializeField] private float maxMovementSpeed = 5.0f;
    [SerializeField] private float laneWidth = 1.5f;
    private Lanes currentLane = Lanes.Center;
    private Rigidbody playerRigidbody;
    private float speedGainCooldown;
    private InputAction moveAction;
    private float moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
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
        
        moveInput = moveAction.ReadValue<float>();
        if(moveInput != 0){Debug.Log(moveInput); }
        //Handle lane switching
        if(moveInput < 0) //Move Left
        {
            if(currentLane == Lanes.Center)
            {
                currentLane = Lanes.Left;
                playerRigidbody.position = new Vector3(playerRigidbody.position.x - laneWidth, playerRigidbody.position.y, playerRigidbody.position.z);
            }
            else if(currentLane == Lanes.Right)
            {
                currentLane = Lanes.Center;
                playerRigidbody.position = new Vector3(playerRigidbody.position.x - laneWidth, playerRigidbody.position.y, playerRigidbody.position.z);
            }
            else
            {
                //Stumble eventually
                Debug.Log("Already in Left Lane");
            }
        }
        else if(moveInput > 0) //Move Right
        {
            if(currentLane == Lanes.Center)
            {
                currentLane = Lanes.Right;
                playerRigidbody.position = new Vector3(playerRigidbody.position.x + laneWidth, playerRigidbody.position.y, playerRigidbody.position.z);
            }
            else if(currentLane == Lanes.Left)
            {
                currentLane = Lanes.Center;
                playerRigidbody.position = new Vector3(playerRigidbody.position.x + laneWidth, playerRigidbody.position.y, playerRigidbody.position.z);
            }
            else
            {
                //Stumble eventually
                Debug.Log("Already in Right Lane");
            }
        }
    }
}
