using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryMovementController : MonoBehaviour
{

    public GameObject player;
    private Rigidbody RB;

    public float acceleration;
    public float decceleration;
    public float strafeSpeed;
    public float rotateSpeed;


    // Start is called before the first frame update
    void Start()
    {
        RB = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            RB.AddRelativeForce(Vector3.up * acceleration);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            RB.AddRelativeForce(Vector3.up * decceleration);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            RB.AddRelativeForce(Vector3.forward * -strafeSpeed);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            RB.AddRelativeForce(Vector3.forward * strafeSpeed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            RB.AddRelativeForce(Vector3.right * -strafeSpeed);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            RB.AddRelativeForce(Vector3.right * strafeSpeed);
        }

        if (Input.GetKey(KeyCode.W))
        {
            player.transform.Rotate(new Vector3(rotateSpeed, 0, 0));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            player.transform.Rotate(new Vector3(-rotateSpeed, 0, 0));
        }

        if (Input.GetKey(KeyCode.A))
        {
            player.transform.Rotate(new Vector3(0,0,rotateSpeed));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            player.transform.Rotate(new Vector3(0,0,-rotateSpeed));
        }
    }
}
