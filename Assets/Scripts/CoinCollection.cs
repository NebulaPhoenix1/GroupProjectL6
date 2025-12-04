using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    [SerializeField] private GameMaster gameMaster;
    private Collider coinCollider;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameMaster.IncrementCollectiblesGained();
        }
        Destroy(gameObject);
    }

    void Start()
    {
        coinCollider = GetComponent<Collider>();
    }
}
