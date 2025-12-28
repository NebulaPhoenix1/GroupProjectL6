using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] private bool instantGameOver = false;
    [SerializeField] private GameObject debrisParticles;

    private void Awake()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
        if(collision.gameObject.CompareTag("Player"))
        {
            if (!playerMovement.GetIsPlayerDashing())
            {
                //Notify player of collision
                if (instantGameOver)
                {
                    collision.gameObject.GetComponent<PlayerMovement>().OnGameOver.Invoke();
                    Debug.Log("Player Collision: Game Over");
                }
                else
                {
                    collision.gameObject.GetComponent<PlayerMovement>().OnStumble.Invoke();
                    Debug.Log("Player Collision: Stumble");
                }
            }
            //destroy obstacle on collision with player if they're dashing instead of triggering gameover/stumbling
            else if(playerMovement.GetIsPlayerDashing())
            {
                Instantiate(debrisParticles, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity);
                GameObject.Destroy(this.gameObject);
            }
        }
        else
        {
            Debug.Log("Road work ahead? uh yeah i sure hope it does");
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
