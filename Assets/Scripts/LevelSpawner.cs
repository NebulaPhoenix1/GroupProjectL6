using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class LevelSpawner : MonoBehaviour
{
    /* 
        Takes a set of predesigned level segments and spawns them in a sequence to create a continuous level.
        Each level segment a 3D prefab of the same length that can be connected end-to-end
    */
    [SerializeField] private GameObject[] levelSegments; // Array of level segment prefabs
    private Vector3 nextSpawnPoint = Vector3.zero; // Position to spawn the next segment
    private Queue<GameObject> completedSegments = new Queue<GameObject>();
    private int segmentDespawnDistance = 3; //How many segments need to be passed before despawning starts

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (levelSegments.Length > 0)
        {
            for(int i = 0; i < 5; i++)
            {
                SpawnLevel();   
            }
        }
        else {  Debug.LogError("No level segments assigned to LevelSpawner."); }
    }

    public void SpawnLevel()
    {
        //Select a random segment and spawn it at the next spawn point
        int selectedIndex = Random.Range(0, levelSegments.Length);
        GameObject segment = Instantiate(levelSegments[selectedIndex], nextSpawnPoint, Quaternion.identity);
        //Update next spawn point for the next segment
        nextSpawnPoint += new Vector3(0, 0, 5);
    }

    public void CompleteSegment(GameObject segment)
    {
        completedSegments.Enqueue(segment);
        // Optionally, you can destroy old segments to free up memory
        Debug.Log("Queue Length: " + completedSegments.Count);
        if (completedSegments.Count > segmentDespawnDistance)
        {
            GameObject oldSegment = completedSegments.Dequeue();
            Destroy(oldSegment);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
