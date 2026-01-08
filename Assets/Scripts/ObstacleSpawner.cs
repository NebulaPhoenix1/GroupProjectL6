using UnityEngine;

//Please see ObjectSpawner.cs for base class details

public class ObstacleSpawner : ObjectSpawner
{
    protected override void Start()
    {
        base.Start(); //Calls base start in ObjectSpawner.cs
    }

    protected override void Update()
    {
        base.Update(); //Calls base update in ObjectSpawner.cs
        // Only check immediately after we try to spawn something
        if (transform.childCount == 0 && prefabList.Length > 0)
        {
            //SpawnObject();
        }
        
    }

    protected override void Awake()
    {
        base.Awake(); //Calls base awake in ObjectSpawner.cs
    }

    public override void SpawnObject()
    {
        
        base.SpawnObject();
    }
}
