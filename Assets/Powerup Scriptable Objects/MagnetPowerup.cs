using UnityEngine;

[CreateAssetMenu(menuName = "Powerup/Magnet Powerup")]
public class MagnetPowerup : PowerUpEffect
{
    [SerializeField] private float radius = 5f;
    [SerializeField] private float attractionSpeed = 5f;
    public override void OnActivate(GameObject target)
    {
        Debug.Log("Magnet Powerup activated!");
    }

    public override void OnTick(GameObject target)
    {
        Debug.Log("Magnet Powerup ticked!");
        //Use find objects by tag to find all collectibles named coins in the scene
        //If within radius, move towards player
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Coin");
        foreach (GameObject collectible in collectibles)
        {
            float distance = Vector3.Distance(target.transform.position, collectible.transform.position);
            if (distance <= radius)
            {
                //Move towards player
                Vector3 direction = (target.transform.position - collectible.transform.position).normalized;
                collectible.transform.position += direction * Time.deltaTime * attractionSpeed; //5f is the speed of attraction
            }
        }
    }

    public override void OnDeactivate(GameObject target)
    {
        Debug.Log("Magnet Powerup deactivated!");
    }
}
