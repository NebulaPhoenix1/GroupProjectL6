using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScores;
    [SerializeField] private GameMaster gameMaster;

    // Update is called once per frame
    void Update()
    {
        if(gameMaster && scoreText &&  highScores)
        {
            scoreText.text = "Score: " + gameMaster.GetCurrentScore().ToString();
            if (gameMaster.HasAchievedHighScore())
            {
                highScores.text = "High Score: " + gameMaster.GetHighScore().ToString();
            }
        }
    }

    public void EnableUI()
    {
        int highScore = gameMaster.GetHighScore();
        highScores.text = "High Score: " + highScore.ToString();
        Debug.Log(highScore);
    }
}
