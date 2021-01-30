using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public Transform[] slots;
    public FoodItemPickup[] foodSlots;
    bool[] isSlotUsed;

    // 2 items at a time

    // Start is called before the first frame update
    void Start()
    {
        foodSlots = new FoodItemPickup[slots.Length];
        isSlotUsed = new bool[slots.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(FoodItemPickup foodItemPickup)
    {
        for (int i = 0; i < isSlotUsed.Length; i++)
        {
            if (isSlotUsed[i] == false)
            {
                isSlotUsed[i] = true;
                foodSlots[i] = foodItemPickup;
                slots[i].GetComponentsInChildren<Image>()[1].sprite = foodItemPickup.foodItem.uiArt;
                var copyColor = slots[i].GetComponentsInChildren<Image>()[1].color;
                copyColor.a = 1;
                slots[i].GetComponentsInChildren<Image>()[1].color = copyColor;
                break;
            }
        }
    }

    public void RemoveItem(FoodItemPickup foodItemPickup)
    {
        for (int i = 0; i < isSlotUsed.Length; i++) {
            if (isSlotUsed[i] && foodItemPickup == foodSlots[i]) {
                isSlotUsed[i] = false;
                foodSlots[i] = null;
                var copyColor = slots[i].GetComponentsInChildren<Image>()[1].color;
                copyColor.a = 0;
                slots[i].GetComponentsInChildren<Image>()[1].color = copyColor;
                slots[i].GetComponentsInChildren<Image>()[1].sprite = null;
            }
        }
    }
}
