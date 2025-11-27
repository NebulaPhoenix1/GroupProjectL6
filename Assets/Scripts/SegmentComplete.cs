using UnityEngine;
using UnityEngine.Events;

public class SegmentComplete : MonoBehaviour
{
    public UnityEvent CompletedSegment; //This unity event is invoked when the segment is completed
                                        //Level generation system will listen to this event and spawn a new segment
    private LevelSpawner levelSpawner;

    void Start()
    {
        levelSpawner = FindFirstObjectByType<LevelSpawner>();
        if(levelSpawner != null)
        {
            CompletedSegment.AddListener(levelSpawner.SpawnLevel);
            Debug.Log("LevelSpawner found and listener added in SegmentComplete script on: " + gameObject.name);
        }
        else
        {
            Debug.LogError("LevelSpawner not found in the scene by: " + gameObject.name + " in SegmentComplete script.");
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);
        if(other.CompareTag("Player"))
        {
            CompletedSegment.Invoke();
        }
    }
}
