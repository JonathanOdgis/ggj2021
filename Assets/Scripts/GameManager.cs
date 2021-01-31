using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public float timeRemaining = 60;
    public float score = 0;
    public bool timerIsRunning = false;

    public int requiredScore = 8;

    public TMP_Text timeText;

    public TMP_Text scoreText;

    public TMP_Text resultsScoreText;

    public GameObject results;

    [SerializeField]
    public TypingGame typingGame;

    public enum GameStates
    {
        START, OVERWORLD, TYPING, WIN, LOSE
    }

    public GameStates gameState;


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        //FloatingTextController.Initialize();
        //results.SetActive(false);
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                //launcher.SetControl(false);
                timeRemaining = 0;
                timerIsRunning = false;
                //results.SetActive(true);
                
            }
        }

        if (score == requiredScore)
        {
            gameState = GameStates.WIN;
        }

        DisplayTime(timeRemaining);
        DisplayScore(score);
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("TIME: {0:00}:{1:00}", minutes, seconds);
    }

    void DisplayScore(float score)
    {
        scoreText.text = "Returns: " + score + "/" + requiredScore;
    }

    public void UpdateScore(float points)
    {
        score += points;
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3f);
        //launcher.SetControl(true);
        timerIsRunning = true;
    }

    public void StartTypingBattle(Customer customer)
    {
        gameState = GameStates.TYPING;
        // Pass all the attributes to the Typing Battle UI to start things up
        typingGame.StartNewGame(customer);
    }

    public void OnTypingBattleFinished()
    {
        gameState = GameStates.OVERWORLD;
    } 
}
