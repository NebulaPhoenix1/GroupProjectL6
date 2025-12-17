using UnityEngine;
using UnityEditor;

//This script has been taken and tweaked from a past project of Luke's and Leyton's :)

public class PlayerPrefsEditor : EditorWindow
{
    // Variable to hold the value while we edit it in the window
    private int editedHigh;
    private int editedCollectible;
    private int editedLastScore;
    private const string highScoreKeyName = "HighScore";
    private const string collectibleKeyName = "Collectibles";
    private const string lastScoreKeyName = "Last Score";

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
        if (PlayerPrefs.HasKey(highScoreKeyName))
        {
            editedHigh = PlayerPrefs.GetInt(highScoreKeyName);
        }
        if (PlayerPrefs.HasKey(collectibleKeyName))
        {
            editedCollectible = PlayerPrefs.GetInt(collectibleKeyName);
        }
        if (PlayerPrefs.HasKey(lastScoreKeyName))
        {
            editedLastScore = PlayerPrefs.GetInt(lastScoreKeyName);
        }
    }

    void OnGUI()
    {
        //Last score editor
        GUILayout.Label("Last Score Settings", EditorStyles.boldLabel);
        // Display the current saved value for reference
        EditorGUILayout.LabelField("Current Last Score:", PlayerPrefs.GetInt(lastScoreKeyName, 0).ToString());
        EditorGUILayout.Space();
        editedLastScore = EditorGUILayout.IntField("New Previous Score:", editedLastScore);
        EditorGUILayout.Space();
        //Save Button
        if (GUILayout.Button("Save Last Score"))
        {
            PlayerPrefs.SetInt(lastScoreKeyName, editedLastScore);
            PlayerPrefs.Save();
            Debug.Log($"Last Score updated to: {lastScoreKeyName}");
        }
        //Reset Button
        if (GUILayout.Button("Reset Last Score"))
        {
            PlayerPrefs.DeleteKey(lastScoreKeyName);
            editedHigh = 0;
            Debug.Log("Last Score key deleted.");
        }

        //High Score Editor
        GUILayout.Label("High Score Settings", EditorStyles.boldLabel);
        // Display the current saved value for reference
        EditorGUILayout.LabelField("Current High Score:", PlayerPrefs.GetInt(highScoreKeyName, 0).ToString());
        EditorGUILayout.Space();
        editedHigh = EditorGUILayout.IntField("New High Score:", editedHigh);
        EditorGUILayout.Space();
        //Save Button
        if (GUILayout.Button("Save High Score"))
        {
            PlayerPrefs.SetInt(highScoreKeyName, editedHigh);
            PlayerPrefs.Save(); 
            Debug.Log($"High Score updated to: {editedHigh}");
        }
        //Reset Button
        if (GUILayout.Button("Reset Highscore"))
        {
            PlayerPrefs.DeleteKey(highScoreKeyName);
            editedHigh = 0;
            Debug.Log("High Score key deleted.");
        }

        //Collectibles Editor
        GUILayout.Label("Collectibles Settings", EditorStyles.boldLabel);
        // Display the current saved value for reference
        EditorGUILayout.LabelField("Current Collectibles Value:", PlayerPrefs.GetInt(collectibleKeyName, 0).ToString());
        EditorGUILayout.Space();  
        editedCollectible = EditorGUILayout.IntField("New Collectibles Value:", editedCollectible);
        EditorGUILayout.Space();
        //Save Button
        if (GUILayout.Button("Save Collectibles"))
        {
            PlayerPrefs.SetInt(collectibleKeyName, editedCollectible);
            PlayerPrefs.Save(); 
            Debug.Log($"Collectible count updated to: {editedCollectible}");
        }
        //Reset Button
        if (GUILayout.Button("Reset Collectibles"))
        {
            PlayerPrefs.DeleteKey(collectibleKeyName);
            editedCollectible = 0;
            Debug.Log("Collectibles key deleted.");
        }
    }
}