using UnityEngine;
using System;
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

    private float rawScore;
    private float scoreOffset;
    private bool gameplayStarted;
    private bool highScoreAchieved = false;
    private int currentScore = 0;
    private int highScore = 0;
    private int collectiblesGained = 0;

    private Transform playerTransform;
    private LevelSpawner levelSpawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rawScore = 0;
        scoreOffset = 0;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        levelSpawner = GameObject.Find("Level Spawner").GetComponent<LevelSpawner>();
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        Debug.Log("Highscore:" +  highScore.ToString());
        highScoreAchieved = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.MainMenu)
        {
            gameplayStarted = false;
            scoreOffset = Time.time;
            //Debug.Log("ScoreOffset: " + scoreOffset);
            return;
        }
        else
        {
            gameplayStarted = true;
            rawScore = (Time.time - scoreOffset) * levelSpawner.GetSpeed();
            currentScore = Convert.ToInt32(rawScore);
            //Debug.Log("CurrentScore: " + currentScore + " Time: " + Time.time + " Raw Score: " + rawScore);
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
        OnGameStart.Invoke();
    }

    void OnDestroy()
    {
        SaveValues();          
    }

    public bool HasAchievedHighScore()
    {
        return highScoreAchieved; 
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public int GetHighScore()
    {
        return highScore; 
    }

    public bool GetGameplayState()
    {
        return gameplayStarted;
    }

    public void IncrementCollectiblesGained()
    {
        collectiblesGained++;
        SaveValues();
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

    //This gets called when OnGameOver unity event in playerMovement.cs is invoked
    public void OnGameOver()
    {
        int lastScore = currentScore;
        SaveValues();
        //Save last score seperately to ensure the last score is always accurate and not saved mid run.
        PlayerPrefs.SetInt("Last Score", lastScore);
        PlayerPrefs.Save();
    }
}
