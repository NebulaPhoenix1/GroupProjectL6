using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CoinSpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject activeCoin;

    //Local position so the coin moves with the platform/parent
    private Vector3 initialLocalPosition;
    private bool positionCaptured = false;
    private UpgradeManager upgradeManager;

    //I'm throwing powerups in here for now as it's easier than making a separate powerup system
    //And with the scope of the project/time remaining it'll have to be fine
    [SerializeField] private UpgradeSciptableItem magnetUpgrade;
    [SerializeField] private GameObject magnetPrefab;
    //Should be between 0-1
    [Range(0, 1)]
    [SerializeField] private float magnetSpawnChance = 0.005f; //0.5% chance to spawn a magnet instead of a coin
    private bool spawnMagnets = false;

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
        upgradeManager = GameObject.Find("Upgrades Manager").GetComponent<UpgradeManager>();
        //Check if we have the magnet upgrade 
        if (upgradeManager.IsUpgradePurchased(magnetUpgrade))
        {
            spawnMagnets = true;
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
        if(spawnMagnets && UnityEngine.Random.value < magnetSpawnChance && magnetPrefab != null)
        {
            //Spawn a magnet powerup instead of a coin
            activeCoin = Instantiate(magnetPrefab, transform);
            activeCoin.transform.localPosition = initialLocalPosition;

            Debug.Log("Spawned a magnet powerup");
            return;
        }
        activeCoin = Instantiate(coinPrefab, transform);
        activeCoin.transform.localPosition = initialLocalPosition;

        //Debug.Log("Spawned a new coin");
    }
}