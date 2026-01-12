using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    private UpgradeManager upgradeManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get reference to Upgrade Manager
        upgradeManager = FindFirstObjectByType<UpgradeManager>();
        
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

            //Add CheckButtonState to the purchase button on click event
            purchaseButton.GetComponent<Button>().onClick.AddListener(CheckButtonState);
            CheckButtonState();
        }
        else
        {
            Debug.LogError("One or more UI components or the upgrade item is not assigned in UpgradePanelUI on " + gameObject.name);
        }
    }

    //Checks with Upgrade Manager to see if this upgrade is unlocked or not and sets button states accordingly
    //This should get called on start and after an attempted purchase
    private void CheckButtonState()
    {
        Debug.Log("Check button state called");
        if(upgradeManager.IsUpgradePurchased(upgradeItem))
        {
            //We own this upgrade, grey out the button
            purchaseButton.SetActive(false);
            greyedOutButton.SetActive(true);
        }
        else
        {
            //We don't own this upgrade, enable purchase button
            purchaseButton.SetActive(true);
            greyedOutButton.SetActive(false);
        }
    }
}
