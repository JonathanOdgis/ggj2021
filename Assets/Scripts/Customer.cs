using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customer", menuName = "Customer")]
public class Customer : ScriptableObject
{
    public string name;
    public Sprite normalSprite;
    public Sprite angrySprite;

    [TextArea(3, 10)]
    public string[] dialogues;

    [TextArea(3, 10)]
    public string[] responses;

    [TextArea(3, 10)]
    public string mistakeStatement;

    [TextArea(3, 10)]
    public string victoryStatement;

    [TextArea(3, 10)]
    public string loseStatement;

}
