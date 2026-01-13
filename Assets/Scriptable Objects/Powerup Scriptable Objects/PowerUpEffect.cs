using UnityEngine;

public abstract class PowerUpEffect : ScriptableObject
{
    public abstract void OnActivate(GameObject target);
    public abstract void OnTick(GameObject target);
    public abstract void OnDeactivate(GameObject target);
}
