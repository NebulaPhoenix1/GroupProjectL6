using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelSpawner : MonoBehaviour
{
    /* 
        Takes a set of predesigned level segments and spawns them in a sequence to create a continuous level.
        Each level segment is created using tilemaps for easy design and modification.
    */

    [SerializeField] private GameObject[] levelSegments; // Array of level segment prefabs
    [SerializeField] private int levelLength = 5; // Number of segments to spawn


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (levelSegments.Length > 0) { SpawnLevel(); }
        else {  Debug.LogError("No level segments assigned to LevelSpawner."); }
    }

    private void SpawnLevel()
    {
        //Spawns each level sequence as a child of the LevelSpawner object (Grid)
        //We need to calculate how many tiles long each segment is to position them correctly
        float segmentWidth = levelSegments[0].GetComponentInChildren<Tilemap>().size.x; 
        for (int i = 0; i < levelLength; i++)
        {
            int randomIndex = Random.Range(0, levelSegments.Length);
            Vector3 spawnPosition = new Vector3(i * segmentWidth, 0, 0);
            Instantiate(levelSegments[randomIndex], spawnPosition, Quaternion.identity, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
