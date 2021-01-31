using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnItemGenerator : MonoBehaviour
{

    [SerializeField]
    FoodItemPickup pickupTemplate;

    public FoodItem[] foodItems;

    public bool[] slotsUsed;

    public FoodItemPickup[] pickups;

    public Transform[] pickupSpots;

    // Start is called before the first frame update
    void Start()
    {
        slotsUsed = new bool[pickupSpots.Length];
        pickups = new FoodItemPickup[pickupSpots.Length];
        StartCoroutine(GenerateFoodItem());
    }


    int maxSpawnsAllowed = 2;
    IEnumerator GenerateFoodItem()
    {
        while (true)
        {
            Debug.Log("Jonny Note - Generate");
            int spawn = 0;
            for (int i = 0; i <= slotsUsed.Length - 1; i++)
            {
                if (spawn > maxSpawnsAllowed)
                    break;
                if (slotsUsed[i] && pickups[i] == null)
                {
                    slotsUsed[i] = false;
                    pickups[i] = null;
                }
                if (!slotsUsed[i] && pickups[i] == null)
                {
                    Debug.Log("Put em in");
                    // Put a food item here.
                    GameObject newPickup = Instantiate(pickupTemplate.gameObject, pickupSpots[i].transform.position, pickupSpots[i].transform.rotation);
                    newPickup.GetComponent<FoodItemPickup>().foodItem = foodItems[Random.Range(0, foodItems.Length)];
                    pickups[i] = newPickup.GetComponent<FoodItemPickup>();
                    slotsUsed[i] = true;
                    spawn += 1;
                }

            }
            yield return new WaitForSeconds(15f);
        }
    }



}
