using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Item Definition", menuName = "Upgrade System/New Upgrade Item Definition")]
public class UpgradeSciptableItem : ScriptableObject
{
    [Tooltip("Unique identifier for the upgrade, used when saving and loading.")]
    public string upgradeID;
    [Tooltip("Name of the upgrade to be displayed in the UI.")]
    public string displayedUpgradeName;
    [Tooltip("Description of the upgrade to be displayed in the UI.")]
    [TextArea(3, 5)]
    public string upgradeDescription;
    [Tooltip("How many coins the upgrade will cost to purchase.")]
    public int upgradeCost;
    [Tooltip("Optional icon to represent the upgrade in the UI.")]
    public Sprite upgradeIcon;
}
