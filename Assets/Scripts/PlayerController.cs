using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    PlayerInventory inventory;

    public GameManager gameManager;

    Rigidbody rgb;

    [SerializeField]
    [Range(0.0f, 1000.0f)]
    float cartForce = 100;

    [SerializeField]
    [Range(0.0f, 1000.0f)]
    float cartDrag = 0.98f;


    [SerializeField]
    ParticleSystem driftFX;

    [SerializeField]
    LayerMask whatIsGround;

    SpriteRenderer playerSprite;

    [SerializeField]
    Sprite forwardSprite;

    [SerializeField]
    Sprite backSprite;


    // Start is called before the first frame update
    void Start()
    {
        rgb = GetComponent<Rigidbody>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.gameState == GameManager.GameStates.TYPING)
        {
            rgb.velocity = Vector3.zero;
            return;
        }

        // Handle Movement Speed
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        dir = dir.normalized * Time.deltaTime;

        rgb.AddForce(dir * cartForce);

        rgb.velocity *= cartDrag;

        // Handle Rotation
        if (dir.x != 0 || dir.z != 0)
        {
            //create the rotation we need to be in to look at the target
            var _lookRotation = Quaternion.LookRotation(dir);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 20);
        }

        float angle = 90;
        if (Mathf.Abs(Vector3.Angle(transform.forward, Camera.main.transform.position - transform.position)) < angle)
        {
            playerSprite.sprite = forwardSprite;
        }
        else
        {
            playerSprite.sprite = backSprite;
        }

        if (dir.x !=0 && dir.z != 0)
        {
            if (driftFX.isStopped)
                driftFX.Play();
        }
        else
        {
            if (driftFX.isPlaying)
                driftFX.Stop();

        }

        // Handle Raycasting
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, 1, whatIsGround))
        {
            rgb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            rgb.constraints = RigidbodyConstraints.FreezeRotationZ;

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FoodItemPickup>())
        {
            var foodItemPickup = other.GetComponent<FoodItemPickup>();
            foodItemPickup.OnPickup();
            inventory.AddItem(foodItemPickup);
        }
        if (other.GetComponent<ItemDropOffPoint>())
        {
            var itemDropOffPoint = other.GetComponent<ItemDropOffPoint>();
            itemDropOffPoint.CheckForItems(inventory);
        }
        if (other.GetComponent<EnemyEncounterCharacter>())
        {
            var enemy = other.GetComponent<EnemyEncounterCharacter>();
            enemy.OnPlayerTrigger();
            gameManager.StartTypingBattle(enemy.customer);
        }
    }
}
