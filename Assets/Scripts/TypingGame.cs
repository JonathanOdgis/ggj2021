using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypingGame : MonoBehaviour
{

    public GameManager gameManager;

    [SerializeField]
    GameObject container;

    [SerializeField]
    Image customerSprite;

    Sprite customerNormalSprite;
    Sprite customerAngrySprite;

    [SerializeField]
    TMP_Text customerName;

    [SerializeField]
    GameObject responsePromptWindow;

    [SerializeField]
    TMP_Text responsePrompt;

    [SerializeField]
    TMP_Text yourResponsePrompt;

    [SerializeField]
    float customerDialogueSpeed = .2f;

    string stringToSubmit = "";

    string originalResponsePromptString = "This is what we gotta type";

    Customer customer;

    [SerializeField]
    TMP_Text customerDialogueText;

    [SerializeField]
    Animator containerAnim;

    public enum ResponseResult
    {
        CORRECT, WRONG, SKIP
    }

    public enum Ranks
    {
        A_RANK, B_RANK, C_RANK
    }

    bool allResponsesCorrect = true;


    // Start is called before the first frame updated
    void Start()
    {
        container.SetActive(false);
        responsePromptWindow.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        if (!responsePromptWindow.gameObject.activeSelf)
            return;

        var input = Input.inputString;

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (stringToSubmit.Length > 0)
            {
                stringToSubmit = stringToSubmit.Substring(0, stringToSubmit.Length - 1);
                if (stringToSubmit.Length < originalResponsePromptString.Length) {
                    char[] arr = yourResponsePrompt.text.ToCharArray();
                    char[] arrOrig = originalResponsePromptString.ToCharArray();

                    arr[stringToSubmit.Length] = arrOrig[stringToSubmit.Length];
                    yourResponsePrompt.text = new string(arr);
                }
            }
            return;
        } else if (Input.GetKey(KeyCode.Backspace))
        {
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (stringToSubmit.Length < originalResponsePromptString.Length)
            {
                char[] arr = yourResponsePrompt.text.ToCharArray();

                arr[stringToSubmit.Length] = ' ';

                yourResponsePrompt.text = new string(arr);

            }
            stringToSubmit += " ";
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Submitting This String: " + stringToSubmit);

            if (stringToSubmit == originalResponsePromptString)
            {
                Debug.Log("Correct");
                NextSentence(ResponseResult.CORRECT);
            }
            else
            {
                Debug.Log("Wrong");
                NextSentence(ResponseResult.WRONG);
            }
            return;
        }

        if (!string.IsNullOrEmpty(input))
        {
            foreach (char inChar in input) {
                if (stringToSubmit.Length < originalResponsePromptString.Length)
                {
                    char[] arr = yourResponsePrompt.text.ToCharArray();

                    arr[stringToSubmit.Length] = ' ';

                    yourResponsePrompt.text = new string(arr);
                }

                stringToSubmit += inChar;
            }
        }

    }

    float gameStartTime;
    float gameEndTime;

    public void StartNewGame(Customer customer)
    {
        Debug.Log("Starting New Game");

        gameEndTime = 0;
        gameStartTime = Time.time;

        allResponsesCorrect = true;

        containerAnim.SetBool("isEnd", false);
        isEnd = false;

        responsePromptWindow.SetActive(false);

        customerDialogueIndex = 0;
        customerDialogueText.text = "";

        this.customer = customer;
        customerNormalSprite = this.customer.normalSprite;
        customerAngrySprite = this.customer.normalSprite;
        customerSprite.sprite = this.customerNormalSprite;

        customerName.text = customer.name;

        // Start Dialogue Cycle

        // Take the customer value and set everything up
        // Start offf with a pretty cool animation sequence.
        container.SetActive(true);

        StartCoroutine(CustomerTyping(customer.dialogues[customerDialogueIndex]));
    }

    public void EndGame()
    {
        // Have customer say one last thing. Then have them run away or fade out
        if (allResponsesCorrect)
            StartCoroutine(CustomerTyping(customer.victoryStatement));
        else
            StartCoroutine(CustomerTyping(customer.loseStatement));
    }

    bool isEnd;

    IEnumerator EndGameSequence()
    {

        Debug.Log("End Game Sequence Called");

        containerAnim.SetBool("isEnd", true);

        yield return new WaitForSeconds(1.5f);

        isEnd = true;

        var overallTime = Time.time - gameStartTime;

        StartCoroutine(CustomerTyping("YOU: Thank you for being a Super Foods Shoppe Customer!"));

        yield return new WaitForSeconds(1.5f);



        container.SetActive(false);
        gameManager.OnTypingBattleFinished();
        FindObjectOfType<AudioManager>().Stop("talking");
        StopAllCoroutines();
    }

    int customerDialogueIndex;
    
    void NextSentence(ResponseResult result)
    {

        // TODO: if wrong give an Angry Dialogue and Sound 
        if (result == ResponseResult.CORRECT)
        {
            FindObjectOfType<AudioManager>().Play("correct_typing");
        }
        else if (result == ResponseResult.WRONG)
        {
            FindObjectOfType<AudioManager>().Play("wrong_typing");
            allResponsesCorrect = false;
            gameManager.UpdateHealth(-1);
        }

        responsePromptWindow.gameObject.SetActive(false);

        stringToSubmit = "";


        if (customerDialogueIndex < customer.dialogues.Length - 1)
        {
            customerDialogueIndex++;
            customerDialogueText.text = "";
            StartCoroutine(CustomerTyping(customer.dialogues[customerDialogueIndex]));
        } else
        {
            customerDialogueIndex = customer.dialogues.Length; // My lazy workaround so the end game can be handled in the Typing Coroutine. 
            customerDialogueText.text = "";
            EndGame();
        }
    }


    IEnumerator TalkSound()
    {
        for (int i = 0; i < 5; i++)
        {
            FindObjectOfType<AudioManager>().Play("talking");
            yield return new WaitForSeconds(.3f);
            FindObjectOfType<AudioManager>().Stop("talking");
        }
    }

    IEnumerator CustomerTyping(string dialogue)
    {

        StartCoroutine(TalkSound());

        customerDialogueText.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            customerDialogueText.text += letter;
            yield return new WaitForSeconds(customerDialogueSpeed);
        }

        if (isEnd)
        {
            yield break;
        }

        // Now Show The Prompt and Setup only if there is anymore response
        if (customerDialogueIndex <= customer.dialogues.Length - 1)
        {
            originalResponsePromptString = customer.responses[customerDialogueIndex];
            responsePrompt.text = originalResponsePromptString;
            yourResponsePrompt.text = originalResponsePromptString;

            if (originalResponsePromptString == "")
            {
                yield return new WaitForSeconds(3f);
                NextSentence(TypingGame.ResponseResult.SKIP);
            }
            else
            {
                responsePromptWindow.gameObject.SetActive(true);
            }
        } else
        {
            StartCoroutine(EndGameSequence());
        }


    }

    public GameObject getContainer()
    {
        return container;
    }

}
