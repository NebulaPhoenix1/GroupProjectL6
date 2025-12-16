using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public class SegmentComplete : MonoBehaviour
{
    public UnityEvent CompletedSegment; //This unity event is invoked when the segment is completed
                                        //Level generation system will listen to this event and spawn a new segment
    private LevelSpawner levelSpawner;
    [SerializeField] private GameObject segmentObject; // Reference to the segment GameObject
    private bool isCompleted = false;
    void Start()
    {

    }
    
    public void OnTriggerEnter(Collider other)
    {
        if(!isCompleted)
        {
            isCompleted = true;
            //Debug.Log("Trigger entered by: " + other.name);
            if(other.CompareTag("Player"))
            {
                CompletedSegment.Invoke();
            }
        }
        else
        {
            //Debug.Log("Segment already completed, ignoring trigger by: " + other.name);
            return;
        }
    }
}
