using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Item", menuName = "FoodItem")]
public class FoodItem : ScriptableObject
{
    public string name;
    public string description;
    public Sprite uiArt;
    
    public enum FoodItemType
    {
        FREEZER, DRINK, DAIRY, BREAD, VEGGIES, FRUITS, CANDY
    }

    public FoodItemType foodItemType;

}
