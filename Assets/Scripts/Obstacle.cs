using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
        if(collision.gameObject.CompareTag("Player"))
        {
            //Notify player of collision
            collision.gameObject.GetComponent<PlayerMovement>().OnStumble.Invoke();
            Debug.Log("Player Collision");
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
