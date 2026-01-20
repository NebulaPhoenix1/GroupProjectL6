using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Item Definition", menuName = "Upgrade System/New Upgrade Item Definition")]
public class UpgradeSciptableItem : ScriptableObject
{
    [Tooltip("Unique identifier for the upgrade, used when saving and loading.")]
    public string upgradeID;

    [Tooltip("Name of the upgrade to be displayed in the UI.")]
    public string displayedUpgradeName;
    
    [Tooltip("Optional icon to represent the upgrade in the UI.")]
    public Sprite upgradeIcon;

    [System.Serializable]
    public struct LevelDefinition
    {
        [TextArea(3,5)]
        [SerializeField] public string levelDescription; //Description for specific level

        public int cost; //Cost for this level
        public float upgradeValue; //Gameplay nunber which is changing e.g. 0.5 for 50% or 90 3 for 3 secs.
    }

    [Tooltip("Add 1 element for single level upgrades or add multiple for multi level upgrades")]
    public LevelDefinition[] levelDefinitions;
}
