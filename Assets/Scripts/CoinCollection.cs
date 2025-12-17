using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    [SerializeField] private GameMaster gameMaster;
    private Collider coinCollider;

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            gameMaster.IncrementCollectiblesGained();
        }
        Destroy(gameObject);
    }

    void Start()
    {
        coinCollider = GetComponent<Collider>();
        if(!gameMaster)
        {
            gameMaster = GameObject.Find("Game Master").GetComponent<GameMaster>();
        }
    }
}
