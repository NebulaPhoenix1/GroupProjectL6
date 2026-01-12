using UnityEngine;
using TMPro;

//This class takes the data in upgrade scriptable object and automatically fills in relevant UI fields.
public class UpgradePanelUI : MonoBehaviour
{
    [Header("Mandatory References")]
    [SerializeField] private UpgradeSciptableItem upgradeItem;
    [SerializeField] private TMP_Text upgradeNameText;
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private GameObject purchaseButton;
    [SerializeField] private GameObject greyedOutButton;
    [Header("Optional References")]
    [SerializeField] private TMP_Text upgradeDescriptionText;
    [SerializeField] private UnityEngine.UI.Image upgradeIconImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Check all mandatory values are assigned
        if(upgradeItem && upgradeNameText &&  upgradeCostText && purchaseButton && greyedOutButton)
        {
            //Assign values from scriptable object to relevant UI components
            upgradeNameText.text = upgradeItem.displayedUpgradeName;
            upgradeCostText.text = upgradeItem.upgradeCost.ToString() + " Coins";
            //Check we have a description/icon and if so enable and set them
            //Description
            if(upgradeDescriptionText && upgradeItem.upgradeDescription != null)
            {
                upgradeDescriptionText.gameObject.SetActive(true);
                upgradeDescriptionText.text = upgradeItem.upgradeDescription;
            }
            else
            {
                upgradeDescriptionText.gameObject.SetActive(false);
                Debug.Log("No description assigned for upgrade: " + upgradeItem.displayedUpgradeName);
            }
            //Icon
            if(upgradeIconImage && upgradeItem.upgradeIcon)
            {
                upgradeIconImage.gameObject.SetActive(true);
                upgradeIconImage.sprite = upgradeItem.upgradeIcon;
            }
            else
            {
                upgradeIconImage.gameObject.SetActive(false);
                Debug.Log("No icon assigned for upgrade: " + upgradeItem.displayedUpgradeName);
            }
            //In future check if we have this uprade unlocked or not and set button states accorindgly
        }
        else
        {
            Debug.LogError("One or more UI components or the upgrade item is not assigned in UpgradePanelUI on " + gameObject.name);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
