using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] private bool instantGameOver = false;
    [SerializeField] private GameObject debrisParticles;
    private GameMaster gameMaster;

    private void Awake()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        gameMaster = FindFirstObjectByType<GameMaster>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collided");
        if(collision.gameObject.CompareTag("Player"))
        {
            if (!playerMovement.GetIsPlayerDashing())
            {
                //Notify player of collision
                if (instantGameOver)
                {
                    //collision.gameObject.GetComponent<PlayerMovement>().TriggerGameOver();
                    collision.gameObject.GetComponent<PlayerMovement>().AttemptStumble();
                    Debug.Log("Player Collision: Stumble");
                    Debug.LogWarning("On GameObject: " + gameObject.name + " Instant Game Over is enabled within Obstacle.cs; this has been disabled and a stumble attempt occured instead.");
                }
                else
                {
                    collision.gameObject.GetComponent<PlayerMovement>().AttemptStumble();
                    Debug.Log("Player Collision: Stumble");
                }
            }
            //destroy obstacle on collision with player if they're dashing instead of triggering gameover/stumbling
            else if(playerMovement.GetIsPlayerDashing())
            {
                Instantiate(debrisParticles, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity);
                //GameObject.Destroy(this.gameObject);
                ObstacleSpawner.ReturnObjectToPool(this.gameObject);
                gameMaster.AwardDashDestructionBonus();
            }
        }
        else if(collision.gameObject.CompareTag("Obstacle"))
        {
            //Debug.LogWarning("Obstacle collided with another obstacle, or potentially itself?");
            ObstacleSpawner.ReturnObjectToPool(collision.gameObject);
            //Debug.Log("Obstacle forced back to pool");
        }
        else
        {
            //Debug.Log("Obstacle collided with non player entity");
        }
    }

    public void Update()
    {
       //Return to pool if obstacle goes off screen
       if(transform.position.z < -10f)
       {
           ObjectSpawner.ReturnObjectToPool(this.gameObject);
       }
    }
}
