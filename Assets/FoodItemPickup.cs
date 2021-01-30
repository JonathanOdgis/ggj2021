using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItemPickup : MonoBehaviour
{
    // Start is called before the first frame update

    public FoodItem foodItem;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = foodItem.uiArt;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPickup()
    {
        gameObject.SetActive(false);
    }

}
