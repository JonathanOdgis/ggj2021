using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    PlayerInventory inventory;

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


    // Start is called before the first frame update
    void Start()
    {
        rgb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
    }
}
