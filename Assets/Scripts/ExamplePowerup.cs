using UnityEngine;

[CreateAssetMenu(menuName = "Powerup/Example Powerup")]
public class ExamplePowerup : PowerUpEffect
{
    public override void OnActivate(GameObject target)
    {
        Debug.Log("Powerup activated!");
    }

    public override void OnTick(GameObject target)
    {
        Debug.Log("Powerup ticked!");
    }

    public override void OnDeactivate(GameObject target)
    {
        Debug.Log("Powerup deactivated!");
    }
}
