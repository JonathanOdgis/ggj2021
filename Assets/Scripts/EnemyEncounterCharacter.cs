using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncounterCharacter : MonoBehaviour
{

    public Customer customer;

    [SerializeField]
    float speed = 20f;

    GameObject player;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>().gameObject;
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!player)
            return;

        if (gameManager && gameManager.gameState != GameManager.GameStates.OVERWORLD)
        {
            return;
        }

        // Or use a box cast and see if inside radius

        if (Vector3.Distance(this.transform.position, player.transform.position) < 20)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position,speed * Time.deltaTime);
        }        
    }

    public void OnPlayerTrigger()
    {
        Destroy(this.gameObject);
    }

    // some detect states
}
