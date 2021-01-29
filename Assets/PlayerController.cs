using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody rgb;

    [SerializeField]
    [Range(0.0f, 1000.0f)]
    float cartForce = 100;

    [SerializeField]
    [Range(0.0f, 1000.0f)]
    float cartDrag = 0.98f;

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

        // Handle Raycasting
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, 1, whatIsGround))
        {
            rgb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            rgb.constraints = RigidbodyConstraints.FreezeRotationZ;

        }




    }
}
