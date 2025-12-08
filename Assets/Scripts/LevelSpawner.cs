using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Unity.Collections;

public class LevelSpawner : MonoBehaviour
{
    /*
    Spawns prefabs in a line which move under the player to create an endless runner effect
    The player only moves left and right, the level moves underneath them
    */    
    [Header("Level Settings")]
    [SerializeField] private GameObject [] levelPrefabs;
    [SerializeField] private float segmentLength;
    [SerializeField] private int initialSegmentCount; 
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f; // Speed the world moves towards camera
    [SerializeField] private float deleteZ = -20f; // The Z position where segments get destroyed
    [SerializeField] private float moveSpeedGainPerSec = 0.01f;
    [SerializeField] private float maxMoveSpeed = 30f;
    
    private GameMaster gameMaster;
    private List<GameObject> spawnedLevels = new List<GameObject>(); 
    private float spawnZ = 0f; 
    
    void Start()
    {
        gameMaster = GameObject.Find("Game Master").GetComponent<GameMaster>();
    private List<GameObject> activeSegments = new List<GameObject>(); 
    private float spawnZ = 0f;

    //Instead of instanting and destroying segments, we're using an object pool
    //This means we can just reactivate and move segments; only Instantiating and Destroying when necessary as these calls are spenny. 
    //The string is the prefab name with the queue of objects being all the instances of that prefab
    private Dictionary<string, Queue<GameObject>> segmentPool = new Dictionary<string, Queue<GameObject>>();

    void Start()
    {
        //Setup the dictionary
        foreach(GameObject prefab in levelPrefabs)
        {
            segmentPool.Add(prefab.name, new Queue<GameObject>());
        }

        // Spawn the initial level segments
        for (int i = 0; i < initialSegmentCount; i++)
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
        spawnZ += segmentLength;
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
            Debug.Log("Reusing segment from pool: " + prefab.name);
            return segment;
        }
        //Otherwise, instantiate a new one
        else
        {
            GameObject segment = Instantiate(prefab);
            segment.name = prefab.name; // Ensure the name matches for pooling
            Debug.Log("Instantiating new segment: " + prefab.name);
            return segment;
        }
    }

    private void ReturnSegmentToPool(GameObject segment)
    {
        segment.SetActive(false);
        if (segmentPool.ContainsKey(segment.name))
        {
            segmentPool[segment.name].Enqueue(segment);
            Debug.Log("Returned segment to pool: " + segment.name);
        }
        else
        {
            Destroy(segment);
            Debug.LogWarning("Attempted to return a segment to a non-existent pool: " + segment.name);
        }
    }

    public float GetSpeed()
    {
        return moveSpeed / 10;
    }
}