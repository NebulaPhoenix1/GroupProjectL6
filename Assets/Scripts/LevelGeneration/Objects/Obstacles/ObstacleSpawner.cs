using NUnit.Framework;
using UnityEngine;

//Please see ObjectSpawner.cs for base class details
//Luke script 
public class ObstacleSpawner : ObjectSpawner
{
    private bool hasAttemptedSpawn = false;
    private GameMaster gameMaster;
    [SerializeField] private Transform spawnTransform;

    protected override void Start()
    {
        base.Start(); //Calls base start in ObjectSpawner.cs
    }

    private void OnEnable()
    {
        hasAttemptedSpawn = false;
    }

    protected override void Update()
    {
        base.Update(); //Calls base update in ObjectSpawner.cs
        // Only check immediately after we try to spawn something
        /*if (gameMaster != null && gameMaster.GetGameplayState())
        {
            if (!hasAttemptedSpawn && transform.childCount == 0 && transform.position.z > 5f)
            {
                Debug.Log("Obstacle Spawner attempting to spawn object again");
                SpawnObject();
                hasAttemptedSpawn = true;
            }
        }*/

    }

    protected override void Awake()
    {
        base.Awake(); //Calls base awake in ObjectSpawner.cs
        gameMaster = GameObject.Find("Game Master").GetComponent<GameMaster>();
    }

    public override void SpawnObject()
    {
        base.SpawnObject();
        spawnedObject.transform.position = spawnTransform.position;
        //spawnedObject.transform.rotation = spawnTransform.rotation;
        //Do a check to see if this object has any children (i.e. spawned obstacle)
        if(transform.childCount == 0)
        {
            hasAttemptedSpawn = false;
            Debug.LogError("Obstacle Spawner has no child objects even after SpawnObject() call");
        }
        else { hasAttemptedSpawn = true; }
    }
}
