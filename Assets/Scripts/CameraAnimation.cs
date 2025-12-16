using UnityEngine;
using Unity.Cinemachine;

public class CameraAnimation : MonoBehaviour
{
    [SerializeField] private GameObject cineCamera;
    public void CameraAnim()
    {
        cineCamera.SetActive(true);
    }
}
