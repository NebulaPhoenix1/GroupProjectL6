using UnityEngine;

public class CoinSpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject activeCoin;

    //Local position so the coin moves with the platform/parent
    private Vector3 initialLocalPosition;
    private bool positionCaptured = false;

    void Awake()
    {
        //Get the position of the coin placed in the editor
        if (activeCoin != null)
        {
            //Get local position to support moving platforms/parents
            initialLocalPosition = activeCoin.transform.localPosition;
            positionCaptured = true;
        }
        else
        {
            //Just in case: Spawn at the script holder's position
            initialLocalPosition = Vector3.zero;
            positionCaptured = true;
        }
    }

    void OnEnable()
    {
        //Debug.Log("Coin spawner enabled");

        // If the coin is missing
        if (activeCoin == null && positionCaptured)
        {
            SpawnCoin();
        }
        //Re enable coin if it was just disabled
        else if (activeCoin != null && !activeCoin.activeInHierarchy)
        {
            activeCoin.SetActive(true);
        }
    }

    void SpawnCoin()
    { 
        activeCoin = Instantiate(coinPrefab, transform);
        activeCoin.transform.localPosition = initialLocalPosition;

        //Debug.Log("Spawned a new coin");
    }
}