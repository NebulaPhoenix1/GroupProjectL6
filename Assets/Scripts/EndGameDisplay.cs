using UnityEngine;
using TMPro;

public class EndGameDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text lastScoreText;
    [SerializeField] private TMP_Text highScoreText;

    //Function to set text values
    public void DispayScores()
    {
        //find the last saved score
        int lastScore = PlayerPrefs.GetInt("Last Score", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        lastScoreText.text = "Score: " + lastScore.ToString();
        highScoreText.text = "High Score: " + highScore.ToString();
    }
}
