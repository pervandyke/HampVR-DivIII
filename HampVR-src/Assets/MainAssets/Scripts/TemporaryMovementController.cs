using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TemporaryMovementController : MonoBehaviour
{

    public GameObject player;
    public GameObject laserSpawner;
    public GameObject laserSpawner2;
    private Rigidbody RB;
    private Quaternion originalRotation;

    public float acceleration;
    public float decceleration;
    public float strafeSpeed;
    public float rotateSpeed;
    public float maxSpeed;

    public float laserSpeed;
    public int laserDamage;

    public int health;

    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean accelerate;
    public SteamVR_Action_Boolean deccelerate;
    public SteamVR_Action_Boolean fire;
    public SteamVR_Action_Boolean resetHeadsetZero;

    [SerializeField]
    private Camera mainCamera;
    private Vector3 headsetZero;
    private Vector3 direction;




    // Start is called before the first frame update
    void Start()
    {
        RB = player.GetComponent<Rigidbody>();
        originalRotation = transform.rotation;
        headsetZero = mainCamera.transform.localPosition;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        print("Headset Location: " + mainCamera.transform.localPosition);
        //Acceleration
        if (Input.GetKey(KeyCode.LeftShift) || GetAccelerateDown() || mainCamera.transform.localPosition.z > headsetZero.z)
        {
            RB.AddRelativeForce(Vector3.forward * acceleration);
        }
        else if (Input.GetKey(KeyCode.LeftControl) || GetDeccelerateDown() || mainCamera.transform.localPosition.z < headsetZero.z)
        {
            RB.AddRelativeForce(Vector3.forward * decceleration);
        }

        //Strafe Up/Down
        if (Input.GetKey(KeyCode.Space) || mainCamera.transform.localPosition.y > headsetZero.y)
        {
            RB.AddRelativeForce(Vector3.up * strafeSpeed);
        }
        else if (Input.GetKey(KeyCode.C) || mainCamera.transform.localPosition.y < headsetZero.y)
        {
            RB.AddRelativeForce(Vector3.up * -strafeSpeed);
        }

        //Strafe Left/Right
        if (Input.GetKey(KeyCode.Z) || mainCamera.transform.localPosition.x < headsetZero.x)
        {
            RB.AddRelativeForce(Vector3.right * -strafeSpeed);
        }
        else if (Input.GetKey(KeyCode.X) || mainCamera.transform.localPosition.x > headsetZero.x)
        {
            RB.AddRelativeForce(Vector3.right * strafeSpeed);
        }

        //Pitch
        if (Input.GetKey(KeyCode.W))
        {
            player.transform.Rotate(new Vector3(rotateSpeed, 0, 0));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            player.transform.Rotate(new Vector3(-rotateSpeed, 0, 0));
        }

        //Roll
        if (Input.GetKey(KeyCode.Q))
        {
            player.transform.Rotate(new Vector3(0,0,rotateSpeed));
        }
        else if (Input.GetKey(KeyCode.E))
        {
            player.transform.Rotate(new Vector3(0,0,-rotateSpeed));
        }

        //Yaw
        if (Input.GetKey(KeyCode.D))
        {
            player.transform.Rotate(new Vector3(0, rotateSpeed, 0));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            player.transform.Rotate(new Vector3(0, -rotateSpeed, 0));
        }
       
        //Reset Control Zero
        if (GetResetHeadsetDown())
        {
            headsetZero = mainCamera.transform.localPosition;
        }

        if (Input.GetKey(KeyCode.B))
        {
            RB.velocity = Vector3.zero;
        }
        if (RB.velocity.magnitude > maxSpeed)
        {
            RB.velocity = Vector3.ClampMagnitude(RB.velocity, maxSpeed);
        }

        RotateToCamera();
    }

    public bool GetAccelerateDown()
    {
        return accelerate.GetState(handType);
    }

    public bool GetDeccelerateDown()
    {
        return deccelerate.GetState(handType);
    }

    public bool GetFireDown()
    {     
        return fire.GetState(handType);
    }

    public bool GetResetHeadsetDown()
    {
        return resetHeadsetZero.GetStateDown(handType);
    }

    private void Update()
    {
        //Shooting
        if (Input.GetKeyDown(KeyCode.F) || GetFireDown())
        {
            Shoot();
        }

        if (health <= 0)
        {
            Destroy(player);
        }
    }

    private void RotateToCamera()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, mainCamera.transform.localRotation, Time.deltaTime * rotateSpeed);
    }

    public void Shoot()
    {
        Vector3 SpawnPosition = laserSpawner.transform.position;
        Quaternion SpawnRotation = laserSpawner.transform.rotation;
        GameObject LaserInstance;
        LaserInstance = Instantiate(Resources.Load<GameObject>("Prefabs/Laser"), SpawnPosition, SpawnRotation) as GameObject;
        LaserInstance.GetComponent<LaserScript>().speed = RB.velocity.magnitude + laserSpeed;
        LaserInstance.GetComponent<LaserScript>().damage = laserDamage;
        SpawnPosition = laserSpawner2.transform.position;
        SpawnRotation = laserSpawner2.transform.rotation;
        LaserInstance = Instantiate(Resources.Load<GameObject>("Prefabs/Laser"), SpawnPosition, SpawnRotation) as GameObject;
        LaserInstance.GetComponent<LaserScript>().speed = RB.velocity.magnitude + laserSpeed;
        LaserInstance.GetComponent<LaserScript>().damage = laserDamage;
    }
}
