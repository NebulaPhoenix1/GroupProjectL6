using System;
using UnityEngine;
//Luke script
public class CoinSpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject activeCoin;
    private Vector3 initialLocalPosition;
    //private bool positionCaptured = false;
    // References
    private UpgradeManager upgradeManager;
    private GameMaster gameMaster;

    [Header("Powerups")]
    [SerializeField] private UpgradeSciptableItem magnetUpgrade;
    [SerializeField] private GameObject magnetPrefab;

    private bool spawnMagnets = false;
    private float magnetSpawnRate;

    void Awake()
    {
        //Capture Position
        if (activeCoin != null)
        {
            initialLocalPosition = activeCoin.transform.localPosition;
            //positionCaptured = true;
        }
        else
        {
            initialLocalPosition = Vector3.zero;
            //positionCaptured = true;
        }

        //Find Managers
        GameObject upgradeManager = GameObject.Find("Upgrades Manager");
        if (upgradeManager) this.upgradeManager = upgradeManager.GetComponent<UpgradeManager>();
        
        GameObject gameMaster = GameObject.Find("Game Master");
        if (gameMaster) this.gameMaster = gameMaster.GetComponent<GameMaster>();


        int currentMagnetSpawnLevel = this.upgradeManager.GetUpgradeCurrentLevel(magnetUpgrade.upgradeID);
        if (this.upgradeManager && currentMagnetSpawnLevel > 0) //If level is greater than 0, upgrade is owned
        {
            spawnMagnets = true;
            magnetSpawnRate = magnetUpgrade.GetValueForLevel(currentMagnetSpawnLevel);
        }
    }

    void Start()
    {
        //Hide existing coin immediately on load
        if (activeCoin != null && gameMaster != null && !gameMaster.GetGameplayState())
        {
            activeCoin.SetActive(false);
        }
    }

    void OnEnable()
    {
        if (gameMaster)
        {
            // Subscribe to the "Start Button" event
            gameMaster.OnGameStart.AddListener(SpawnCoin);
            
            //Only spawn immediately if the game is running.
            if (gameMaster.GetGameplayState())
            {
                SpawnCoin();
            }
        }
    }

    void OnDisable()
    {
        if (gameMaster)
        {
            gameMaster.OnGameStart.RemoveListener(SpawnCoin);
        }
    }

    void SpawnCoin()
    {
        //Handle the hidden coins from start
        if (activeCoin != null)
        {
            // We only turn it on. We assume if it's there, it's the one we hid.
            activeCoin.SetActive(true);
            return;
        }

        //Spawning Logic in case no coin exists at the moment
        if (spawnMagnets && UnityEngine.Random.value < magnetSpawnRate && magnetPrefab != null)
        {
            activeCoin = Instantiate(magnetPrefab, transform);
            activeCoin.transform.localPosition = initialLocalPosition;
            return;
        }

        if (coinPrefab != null)
        {
            activeCoin = Instantiate(coinPrefab, transform);
            activeCoin.transform.localPosition = initialLocalPosition;
        }
    }
}