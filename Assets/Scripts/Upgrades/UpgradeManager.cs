using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//Saves/Loads/Manages upgrades purchased by the player

public class UpgradeManager : MonoBehaviour
{
    
    [SerializeField] private Button[] upgradeButtons;
    //References
    [SerializeField] private GameMaster gameMaster;
    [SerializeField] private UpgradeSciptableItem[] allUpgrades; //List of all possible upgrades in the game
    private static HashSet<string> purchasedUpgrades = new HashSet<string>(); //Set of upgrade IDs that have been purchased
    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/upgrades.json";
        if(purchasedUpgrades == null)
        {
            LoadUpgrades();
        }
    }   

    void Start()
    {
        if(!gameMaster) { Debug.LogError("UpgradeManager: No GameMaster assigned!"); }
        if(allUpgrades.Length == 0) { Debug.LogWarning("UpgradeManager: No upgrades assigned in inspector!"); }
        saveFilePath = Application.persistentDataPath + "/upgrades.json";
        LoadUpgrades();

        for(int i = 0; i < allUpgrades.Length; i++)
        {
            int index = i; 
            if(upgradeButtons.Length > index && upgradeButtons[index] != null)
            {
               upgradeButtons[index].onClick.AddListener(delegate { PurchaseUpgrade(allUpgrades[index]); });
            }
        }
    }

    //Wrapper class for serializing the HashSet
    [System.Serializable]
    private class UpgradeSaveDataWrapper
    {
        public List<string> purchasedUpgradeIDs;
    }

    private void LoadUpgrades()
    {
        //Check if we have save file
        if(File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            UpgradeSaveDataWrapper saveData = JsonUtility.FromJson<UpgradeSaveDataWrapper>(json);
            purchasedUpgrades = new HashSet<string>(saveData.purchasedUpgradeIDs);
        }
        //No file found
        else
        {
            purchasedUpgrades = new HashSet<string>();
            Debug.Log("No upgrade save file found, starting fresh.");
        }
    }

    private void SaveUpgrades()
    {
        UpgradeSaveDataWrapper saveData = new UpgradeSaveDataWrapper();
        saveData.purchasedUpgradeIDs = new List<string>(purchasedUpgrades);  
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveFilePath, json); 
    }

    public void PurchaseUpgrade(UpgradeSciptableItem upgrade)
    {
        //Check if we already own this upgrade
        if(purchasedUpgrades.Contains(upgrade.upgradeID))
        {
            Debug.Log("Upgrade already purchased: " + upgrade.displayedUpgradeName);
            return;
        }
        //If not, check if we can afford it
        //The game master will check if we can spend coins, and will return true if successful and automatically deduct the coins
        bool purchaseSuccessful = gameMaster.TrySpendCollectibles(upgrade.upgradeCost);
        //If so, add to purchased set
        if (purchaseSuccessful)
        {
            purchasedUpgrades.Add(upgrade.upgradeID);
            SaveUpgrades();
            Debug.Log("Purchased upgrade: " + upgrade.displayedUpgradeName);
            return;
        }
        //Else, do nothing.
        return;
    }

    public bool IsUpgradePurchased(UpgradeSciptableItem upgrade)
    {
        return purchasedUpgrades.Contains(upgrade.upgradeID);
    }

}
