using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

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
        foreach (GameObject segment in spawnedLevels)
        {
            segment.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }

        spawnZ -= moveSpeed * Time.deltaTime;
    }

    void CheckForCleanup()
    {
        // Check if the oldest segment (index 0) has moved past the delete threshold
        if (spawnedLevels.Count > 0 && spawnedLevels[0].transform.position.z < deleteZ)
        {
            RemoveOldestSegment();
            SpawnSegment(); // Add a new one at the end to keep the loop going
        }
    }

    void SpawnSegment()
    {
        int selectedPrefabIndex = Random.Range(0, levelPrefabs.Length);
        
        // Instantiate at the current spawnZ position
        GameObject segment = Instantiate(levelPrefabs[selectedPrefabIndex], new Vector3(0, 0, spawnZ), Quaternion.identity);
        
        spawnedLevels.Add(segment);
        spawnZ += segmentLength;
    }

    void RemoveOldestSegment()
    {
        GameObject oldSegment = spawnedLevels[0];
        spawnedLevels.RemoveAt(0); // Remove from list
        Destroy(oldSegment);       // Remove from scene
    }

    public float GetSpeed()
    {
        return moveSpeed / 10;
    }
}