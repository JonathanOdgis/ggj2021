using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public float timeRemaining = 60;
    public float score = 0;
    public float health = 3;

    public bool timerIsRunning = false;

    public int requiredScore = 8;

    public TMP_Text timeText;

    public TMP_Text scoreText;

    public TMP_Text resultsScoreText;

    public GameObject results;

    [SerializeField]
    public TypingGame typingGame;

    [SerializeField]
    public Transform heartsContainer;

    PlayerController player;

    public bool timerDisabled;

    public enum GameStates
    {
        START, OVERWORLD, TYPING, WIN, LOSE
    }

    public GameStates gameState;

    public EnemyEncounterCharacter victoryCharacter;

    public EnemyEncounterCharacter failCharacter;

    GameObject resultChar;

    bool didResultSpawn;


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        //FloatingTextController.Initialize();
        //results.SetActive(false);
        player = GameObject.FindObjectOfType<PlayerController>();
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (!timerDisabled && timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                gameState = GameStates.LOSE;
                //launcher.SetControl(false);
                timeRemaining = 0;
                timerIsRunning = false;
                //results.SetActive(true);
                
            }
        }

        if (timerDisabled)
        {
            timeText.enabled = false;
        }


        if (score == requiredScore)
        {
            gameState = GameStates.WIN;
        }

        if (health == 0)
        {
            gameState = GameStates.LOSE;
        }

        if ((gameState == GameStates.WIN || gameState == GameStates.LOSE) && !didResultSpawn)
        {
            timerIsRunning = false;
            didResultSpawn = true;
            if (gameState == GameStates.WIN)
            {
                resultChar = Instantiate(victoryCharacter.gameObject, player.gameObject.transform.position, player.gameObject.transform.rotation);

            }
            else if (gameState == GameStates.LOSE)
            {
                resultChar = Instantiate(failCharacter.gameObject, player.gameObject.transform.position, player.gameObject.transform.rotation);

            }
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

    public void UpdateHealth(int healthChange)
    {
        health += healthChange;

        var heartIdx = 0;
        foreach (Transform heart in heartsContainer.transform)
        {
            if (health > heartIdx)
            {
                heart.gameObject.SetActive(true);
            }
            else
            {
                heart.gameObject.SetActive(false);
            }
            heartIdx++;
        }

    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3f);
        //launcher.SetControl(true);
        timerIsRunning = true;
    }

    public void StartTypingBattle(Customer customer)
    {
        if (gameState != GameStates.TYPING && gameState == GameStates.OVERWORLD)
        {
            Debug.Log("Typing Starting");
            gameState = GameStates.TYPING;
            // Pass all the attributes to the Typing Battle UI to start things up
            typingGame.StartNewGame(customer);
        }
        else
        {
            Debug.Log("End Game Called for win/lose");
            // Win/Lose Condition

            StartCoroutine(GoToTitle());

            typingGame.StartNewGame(customer);

        }
    }

    IEnumerator GoToTitle()
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("TitleScreen");

    }

    public void OnTypingBattleFinished()
    {
        gameState = GameStates.OVERWORLD;
    } 
}
