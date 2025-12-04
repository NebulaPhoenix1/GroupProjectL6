using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;

public class GameMaster : MonoBehaviour
{
    enum GameState
    {
        MainMenu,
        Gameplay
    }


    //Unity Events
    public UnityEvent OnHighScoreAchieved; //Called when the player gets a highscore in the current run
    public UnityEvent OnGameStart;

    [SerializeField] private GameState gameState;
    [SerializeField] private CinemachineCamera cineCam;

    private bool highScoreAchieved = false;
    private int currentScore = 0;
    private int highScore = 0;
    private int collectiblesGained = 0;
    private Transform playerTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.MainMenu)
        {
            return;
        }
        else
        {
            currentScore = (int)playerTransform.position.z;
            if (currentScore > highScore)
            {
                highScore = currentScore;
                //Only fire high score achieved event once per run
                if (!highScoreAchieved)
                {
                    highScoreAchieved = true;
                    OnHighScoreAchieved.Invoke();
                }
            }
            return;
        }
    }

    public void StartGame()
    {
        gameState = GameState.Gameplay;
    }

    void OnDestroy()
    {
        SaveValues();          
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void IncrementCollectiblesGained()
    {
        collectiblesGained++;
    }

    public void SaveValues()
    {
        if(highScoreAchieved)
        {
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        //Collecibles = Total collectibles collected across all runs
        PlayerPrefs.SetInt("Collectibles", PlayerPrefs.GetInt("Collectibles", 0) + collectiblesGained);
        PlayerPrefs.Save();
    }
}
