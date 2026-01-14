using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource coinCollect;
    [SerializeField] private AudioSource dashing;
    [SerializeField] private AudioSource hardPlayerImpact;
    [SerializeField] private AudioSource obstacleDestroy;

    private float timePassed = 0;

    public void PlayCoinCollect()
    {
        coinCollect.Play();
        coinCollect.pitch += 0.03f;
        timePassed = 0f;
        StartCoroutine(CountTimePassed());
    }

    private IEnumerator CountTimePassed()
    {
        while (timePassed < 1)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        if (timePassed >= 1)
        {
            coinCollect.pitch = 1f;
            yield return null;
        }
    }

    public void PlayDashing()
    {
        dashing.Play();
        dashing.loop = !dashing.loop;
    }
     
    public void PlayHardPlayerImpact()
    {
        hardPlayerImpact.Play();
    }

    public void PlayObstacleDestroy()
    {
        obstacleDestroy.Play();
    }
}
