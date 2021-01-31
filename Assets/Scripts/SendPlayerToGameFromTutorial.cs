using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SendPlayerToGameFromTutorial : MonoBehaviour
{

    GameManager gameManager;

    TypingGame typingGame;

    bool firstTypingHappened = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        typingGame = gameManager.typingGame;
    }

    // Update is called once per frame
    void Update()
    {

        if (gameManager.gameState == GameManager.GameStates.TYPING)
        {
            firstTypingHappened = true;
        }

        if (firstTypingHappened && gameManager.gameState == GameManager.GameStates.OVERWORLD)
            SceneManager.LoadScene("MainScene");
    }
}
