using UnityEngine;
//Luke script, Leyton added audio logic
public class CoinCollection : MonoBehaviour
{
    [SerializeField] private GameMaster gameMaster;
    [SerializeField] private AudioController audioController;
    private Collider coinCollider;
    private Transform coinTransform;
    [SerializeField] private ParticleSystem coinCollectEffect;

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            gameMaster.IncrementCollectiblesGained();
        }

        if (gameMaster.GetGameplayState())
        {
            audioController.PlayCoinCollect();
            Instantiate(coinCollectEffect, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), Quaternion.identity);
        }
        Destroy(gameObject);
    }

    void Awake()
    {
        coinCollider = GetComponent<Collider>();
        coinTransform = GetComponent<Transform>();
        if(!gameMaster)
        {
            gameMaster = GameObject.Find("Game Master").GetComponent<GameMaster>();
        }

        if(!audioController)
        {
            audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
        }
    }
}
