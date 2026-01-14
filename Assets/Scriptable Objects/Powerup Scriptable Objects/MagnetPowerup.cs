using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerup/Magnet Powerup")]
public class MagnetPowerup : PowerUpEffect
{
    [SerializeField] private float radius = 5f;
    [SerializeField] private float attractionSpeed = 5f;
    private GameObject magnetIcon; //The icon to display when the powerup is active
    public override void OnActivate(GameObject target)
    {
        Debug.Log("Magnet Powerup activated!");
        //Find the magnet icon in the player's UI and enable it
        magnetIcon = GameObject.Find("Gameplay UI/Magnet Icon");
        if(magnetIcon != null)
        {
            magnetIcon.SetActive(true);
            Debug.Log("Magnet icon activated in UI");
        }
        else
        {
            Debug.LogError("Magnet icon not found in UI");
        }
    }

    public override void OnTick(GameObject target)
    {
        //Debug.Log("Magnet Powerup ticked!");
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
        if(magnetIcon != null)
        {
            magnetIcon.SetActive(false);
        }
    }
}
