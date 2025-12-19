using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private bool instantGameOver = false;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
        if(collision.gameObject.CompareTag("Player"))
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
