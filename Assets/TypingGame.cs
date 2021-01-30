using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingGame : MonoBehaviour
{
    [SerializeField]
    GameObject container;

    [SerializeField]
    Image customerSprite;

    Sprite customerNormalSprite;
    Sprite customerAngrySprite;

    // Start is called before the first frame update
    void Start()
    {
        container.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame(Customer customer)
    {
        customerNormalSprite = customer.normalSprite;
        customerAngrySprite = customer.normalSprite;
        customerSprite.sprite = customerNormalSprite;
        // Take the customer value and set everything up
        // Start offf with a pretty cool animation sequence.
        container.SetActive(true);

    }
}
