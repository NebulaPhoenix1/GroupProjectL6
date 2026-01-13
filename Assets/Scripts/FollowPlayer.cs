using UnityEngine;

public class FarmerFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerMovement playerScript;


    [SerializeField] private float followDistanceZ = 3.5f; //How far behind the player
    [SerializeField] private float catchDistanceZ = 0.6f;  //Close-up for the Game Over
    [SerializeField] private float laneChangeSpeed = 12f;  //How fast he switches lanes
    [SerializeField] private float jumpForce = 6f;

    private Rigidbody rb;
    private bool isPlayerCaught = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Link into your PlayerMovement events
        playerScript.OnJump.AddListener(FarmerJump);
        playerScript.OnStumble.AddListener(ShowFarmer);
        playerScript.OnRecover.AddListener(HideFarmer);
        playerScript.OnGameOver.AddListener(CatchPlayer);

        //Start the game with the farmer hidden
        gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        //If the player is caught or gone, the farmer stops moving
        if (playerTransform == null || isPlayerCaught) return;

        float targetX = playerTransform.position.x;
        float targetZ = playerTransform.position.z - followDistanceZ;

        float smoothedX = Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * laneChangeSpeed);

        //Update Position
        transform.position = new Vector3(smoothedX, transform.position.y, targetZ);
    }

    private void FarmerJump()
    {
        if (isPlayerCaught) return;

        if (Mathf.Abs(rb.linearVelocity.y) < 0.05f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //farmerAnimator.SetTrigger("Jump");
        }
    }

    private void ShowFarmer()
    {
        isPlayerCaught = false;
        gameObject.SetActive(true);
        //farmerAnimator.SetBool("IsRunning", true);

        transform.position = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z - followDistanceZ);
    }

    private void HideFarmer()
    {
        if (isPlayerCaught) return;
        gameObject.SetActive(false);
    }

    private void CatchPlayer()
    {
        isPlayerCaught = true;

        //Move the farmer right up to the player's back
        Vector3 catchPos = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z - catchDistanceZ);
        transform.position = catchPos;

        //farmerAnimator.SetTrigger("CatchTrigger");
        //farmerAnimator.SetBool("IsRunning", false);
    }
}