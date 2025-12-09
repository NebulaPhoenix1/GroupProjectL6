using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public void Update()
    {
       //Return to pool if obstacle goes off screen
       if(transform.position.z < -10f)
       {
           ObjectSpawner.ReturnObjectToPool(this.gameObject);
       }
    }
}
