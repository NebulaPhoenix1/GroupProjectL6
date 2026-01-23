using UnityEngine;

//This script just holds some data on the segment for level gen
//There shouldn't be any functionality within this script outside of getters/setters
//Luke script
public class SegmentData : MonoBehaviour
{
    [SerializeField] private float segmentLength;

    public float GetSegmentLength()
    {
        return segmentLength; 
    }
}
