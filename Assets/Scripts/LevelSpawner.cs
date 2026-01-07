using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.Rendering.PostProcessing;

public class LevelSpawner : MonoBehaviour
{
    /*
    Spawns prefabs in a line which move under the player to create an endless runner effect
    The player only moves left and right, the level moves underneath them
    */    
    [Header("Level Settings")]
    [SerializeField] private GameObject [] levelPrefabs;
    [SerializeField] private int defaultSegmentLength;
    private float segmentLength;
    [SerializeField] private int menuInitialSegmentCount;
    [SerializeField] private int additionalInitialSegmentCount; 
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f; // Speed the world moves towards camera
    [SerializeField] private float deleteZ = -20f; // The Z position where segments get destroyed
    [SerializeField] private float moveSpeedGainPerSec = 0.01f;
    [SerializeField] private float maxMoveSpeed = 30f;
    
    private GameMaster gameMaster;
    private List<GameObject> spawnedLevels = new List<GameObject>(); 
    private float spawnZ = 0f;
    private List<GameObject> activeSegments = new List<GameObject>(); 
    

    //Instead of instanting and destroying segments, we're using an object pool
    //This means we can just reactivate and move segments; only Instantiating and Destroying when necessary as these calls are spenny. 
    //The string is the prefab name with the queue of objects being all the instances of that prefab
    private Dictionary<string, Queue<GameObject>> segmentPool = new Dictionary<string, Queue<GameObject>>();

    void Start()
    {
        gameMaster = GameObject.Find("Game Master").GetComponent<GameMaster>();
        //Setup the dictionary
        foreach (GameObject prefab in levelPrefabs)
        {
            segmentPool.Add(prefab.name, new Queue<GameObject>());
        }

        // Spawn the initial level segments
        for (int i = 0; i < menuInitialSegmentCount; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        UpdateMoveSpeed();
        MoveSegments();
        CheckForCleanup();
    }

    void UpdateMoveSpeed()
    {
        if (moveSpeed < maxMoveSpeed && gameMaster.GetGameplayState())
        {
            moveSpeed += moveSpeedGainPerSec * Time.deltaTime;
            if (moveSpeed > maxMoveSpeed)
            {
                moveSpeed = maxMoveSpeed;
            }
        }
    }

    public void UpdateSegmentCount()
    {
        //add a specified number of extra segments when play button is pressed
        for (int i = 0; i < additionalInitialSegmentCount; i++)
        {
            SpawnSegment();
        }
    }

    void MoveSegments()
    {
        // 1. Move the actual objects
        foreach (GameObject segment in activeSegments)
        {
            segment.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }

        spawnZ -= moveSpeed * Time.deltaTime;
    }

    void CheckForCleanup()
    {
        // Check if the oldest segment (index 0) has moved past the delete threshold
        if (activeSegments.Count > 0 && activeSegments[0].transform.position.z < deleteZ)
        {
            RemoveOldestSegment();
            SpawnSegment(); // Add a new one at the end to keep the loop going
        }
    }

    void SpawnSegment()
    {
        int selectedPrefabIndex = Random.Range(0, levelPrefabs.Length);
        GameObject selectedPrefab = levelPrefabs[selectedPrefabIndex];
        GameObject segment = GetSegmentFromPool(selectedPrefab);
        segment.transform.position = new Vector3(0, 0, spawnZ);
        segment.SetActive(true);
        activeSegments.Add(segment);

        //Get the individual segment length (this makes it so each segment does not have to be of equal length)
        SegmentData segmentData = segment.GetComponent<SegmentData>();
        if (segmentData != null)
        {
            segmentLength = segmentData.GetSegmentLength();
            spawnZ += segmentLength;
        }
        else
        {
            Debug.LogWarning("Level spawner could not find: " + segment.name + "'s SegementData");
            spawnZ += defaultSegmentLength;
        }


        //If main game has started
        if (gameMaster.GetGameplayState())
        {
            //Iterate through each obstacle in segment and spawn in obstacle
            ObjectSpawner[] spawners = segment.GetComponentsInChildren<ObjectSpawner>();
            foreach (ObjectSpawner spawner in spawners)
            {
                spawner.SpawnObject();
            }
        } 
    }

    void RemoveOldestSegment()
    {
        GameObject oldSegment = activeSegments[0];
        activeSegments.RemoveAt(0); 
        ReturnSegmentToPool(oldSegment);
    }

    private GameObject GetSegmentFromPool(GameObject prefab)
    {
        //If we have an available segment in the pool, reuse it
        if (segmentPool.ContainsKey(prefab.name) && segmentPool[prefab.name].Count > 0)
        {
            GameObject segment = segmentPool[prefab.name].Dequeue();
            segment.SetActive(true);
            //Debug.Log("Reusing segment from pool: " + prefab.name);
            return segment;
        }
        //Otherwise, instantiate a new one
        else
        {
            GameObject segment = Instantiate(prefab);
            segment.name = prefab.name; // Ensure the name matches for pooling
            //Debug.Log("Instantiating new segment: " + prefab.name);
            return segment;
        }
    }

    private void ReturnSegmentToPool(GameObject segment)
    {
        segment.SetActive(false);
        if (segmentPool.ContainsKey(segment.name))
        {
            segmentPool[segment.name].Enqueue(segment);
            //Debug.Log("Returned segment to pool: " + segment.name);
        }
        else
        {
            Destroy(segment);
            //Debug.LogWarning("Attempted to return a segment to a non-existent pool: " + segment.name);
        }
    }

    public float GetSpeed()
    {
        return moveSpeed;
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void SetMenuInitialSegmentCount(int intialCount)
    {
        menuInitialSegmentCount = intialCount;
        CalculateSegmentRatio();
    }

    private void CalculateSegmentRatio()
    {
        additionalInitialSegmentCount = defaultSegmentLength - menuInitialSegmentCount;
    }
}