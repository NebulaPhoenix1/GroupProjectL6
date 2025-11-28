using UnityEngine;
using UnityEditor;

//This script has been taken and tweaked from a past project of Luke's and Leyton's :)

public class PlayerPrefsEditor : EditorWindow
{
    // Variable to hold the value while we edit it in the window
    private int editedHigh;
    private int editedCollectible;
    private const string KeyName = "HighScore";
    private const string CollectibleKeyName = "Collectibles";

    [MenuItem("Tools/Player Preferences")]
    public static void OpenWindow()
    {
        PlayerPrefsEditor window = (PlayerPrefsEditor)EditorWindow.GetWindow(typeof(PlayerPrefsEditor));
        window.titleContent = new GUIContent("Player Preferences");
        window.Show();
    }

    //Load the current value when the window opens
    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(KeyName))
        {
            editedHigh = PlayerPrefs.GetInt(KeyName);
        }
        if (PlayerPrefs.HasKey(CollectibleKeyName))
        {
            editedCollectible = PlayerPrefs.GetInt(CollectibleKeyName);
        }
    }

    void OnGUI()
    {
        //High Score Editor
        GUILayout.Label("High Score Settings", EditorStyles.boldLabel);
        // Display the current saved value for reference
        EditorGUILayout.LabelField("Current High Score:", PlayerPrefs.GetInt(KeyName, 0).ToString());
        EditorGUILayout.Space();
        editedHigh = EditorGUILayout.IntField("New High Score:", editedHigh);
        EditorGUILayout.Space();
        //Save Button
        if (GUILayout.Button("Save High Score"))
        {
            PlayerPrefs.SetInt(KeyName, editedHigh);
            PlayerPrefs.Save(); 
            Debug.Log($"High Score updated to: {editedHigh}");
        }
        //Reset Button
        if (GUILayout.Button("Reset Highscore"))
        {
            PlayerPrefs.DeleteKey(KeyName);
            editedHigh = 0;
            Debug.Log("High Score key deleted.");
        }

        //Collectibles Editor
        GUILayout.Label("Collectibles Settings", EditorStyles.boldLabel);
        // Display the current saved value for reference
        EditorGUILayout.LabelField("Current Collectibles Value:", PlayerPrefs.GetInt(CollectibleKeyName, 0).ToString());
        EditorGUILayout.Space();  
        editedCollectible = EditorGUILayout.IntField("New Collectibles Value:", editedCollectible);
        EditorGUILayout.Space();
        //Save Button
        if (GUILayout.Button("Save Collectibles"))
        {
            PlayerPrefs.SetInt(CollectibleKeyName, editedCollectible);
            PlayerPrefs.Save(); 
            Debug.Log($"Collectible count updated to: {editedCollectible}");
        }
        //Reset Button
        if (GUILayout.Button("Reset Collectibles"))
        {
            PlayerPrefs.DeleteKey(CollectibleKeyName);
            editedCollectible = 0;
            Debug.Log("Collectibles key deleted.");
        }
    }
}