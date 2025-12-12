using UnityEngine;

//When making a powerup, use this file as a template to copy from; please dont add to it

//This line just makes it so you can right click in the asset drawer to create the 
//Scriptable object; make sure to change the menu name string to say the name of your powerup
[CreateAssetMenu(menuName = "Powerup/Example Powerup 2")]

//This class inherits from PowerUpEffect (the template class)
public class ExamplePowerup2 : PowerUpEffect
{
    //You can have any public, private and or serializable values you want for the powerup 
    //They can be changed within the scripable object's inspector panel
    [SerializeField] private float exampleValue;

    //As long as the function definitions within your script look like this,
    //They should work. Obviously, the code within them can be whatever you want
    //And they dont all need to be filled or done. If you just need OnTick thats fine.
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

