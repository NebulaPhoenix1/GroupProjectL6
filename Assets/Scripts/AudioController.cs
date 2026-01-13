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
        CountTimePassed();
    }

    private void CountTimePassed()
    {
        while (timePassed < 1)
        {
            timePassed += Time.deltaTime;
        }
        IncreaseCoinCollectPitch();
    }

    private void IncreaseCoinCollectPitch()
    {
        if (timePassed == 0)
        {
            coinCollect.pitch = 0;
        }
        else
        {
            coinCollect.pitch += 0.1f;
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
