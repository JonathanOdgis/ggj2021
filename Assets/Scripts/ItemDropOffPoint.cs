using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropOffPoint : MonoBehaviour
{

    public FoodItem.FoodItemType requiredFoodType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckForItems(PlayerInventory playerInventory)
    {
        for (int i = 0; i < playerInventory.slots.Length; i++)
        {
            if (!playerInventory.foodSlots[i])
                continue;
            if (playerInventory.foodSlots[i].foodItem.foodItemType == requiredFoodType)
            {
                Debug.Log("Good Jorb You Got The Item");
                playerInventory.RemoveItem(playerInventory.foodSlots[i]);
            }
        }
    }
}
