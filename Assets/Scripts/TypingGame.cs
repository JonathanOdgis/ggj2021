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

    public enum ResponseResult
    {
        CORRECT, WRONG, SKIP
    }

    // Start is called before the first frame update
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

    public void StartNewGame(Customer customer)
    {
        customerDialogueIndex = 0;
        customerDialogueText.text = "";

        this.customer = customer;
        customerNormalSprite = this.customer.normalSprite;
        customerAngrySprite = this.customer.normalSprite;
        customerSprite.sprite = this.customerNormalSprite;

        // Start Dialogue Cycle

        // Take the customer value and set everything up
        // Start offf with a pretty cool animation sequence.
        container.SetActive(true);

        StartCoroutine(CustomerTyping(customer.dialogues[customerDialogueIndex]));
    }

    public void EndGame()
    {
        // Have customer say one last thing. Then have them run away or fade out
        StartCoroutine(CustomerTyping(customer.victoryStatement));

        

    }

    int customerDialogueIndex;
    
    void NextSentence(ResponseResult result)
    {

        // TODO: if wrong give an Angry Dialogue and Sound 

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


    IEnumerator CustomerTyping(string dialogue)
    {
        Debug.Log("Customer Typing...");
        foreach (char letter in dialogue.ToCharArray())
        {
            customerDialogueText.text += letter;
            yield return new WaitForSeconds(customerDialogueSpeed);
        }

        // Now Show The Prompt and Setup only if there is anymore response
        if (customerDialogueIndex <= customer.dialogues.Length - 1)
        {
            originalResponsePromptString = customer.responses[customerDialogueIndex];
            responsePrompt.text = originalResponsePromptString;
            yourResponsePrompt.text = originalResponsePromptString;

            responsePromptWindow.gameObject.SetActive(true);
        } else
        {
            container.SetActive(false);
            gameManager.OnTypingBattleFinished();
        }


    }

}
