using System;
using TMPro;
using UnityEngine;

public class MenuHighScore : MonoBehaviour
{ 
    [SerializeField] private TMP_Text highScoreText;

    //Function to set text values
    void Start()
    {
        //find the saved highscore
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        highScoreText.text = "High Score: " + highScore.ToString();
    }
}
