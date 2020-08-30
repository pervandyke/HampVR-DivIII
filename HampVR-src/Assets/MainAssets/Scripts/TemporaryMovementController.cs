using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Valve.VR;

public class TemporaryMovementController : MonoBehaviour
{

    public GameObject player;
    public GameObject playerModel;
    public GameObject laserSpawner;
    public GameObject laserSpawner2;
    private Rigidbody RB;
    private Quaternion originalRotation;

    public float acceleration;
    public float decceleration;
    public float strafeSpeed;
    public float rotateSpeed;
    public float maxSpeed;

    public AnimationCurve targetSpeedCurve;
    public AnimationCurve targetReverseSpeedCurve;

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
    private Vector3 maxForwardLean;
    private Vector3 maxRearwardLean;




    // Start is called before the first frame update
    void Start()
    {
        RB = player.GetComponent<Rigidbody>();
        originalRotation = transform.rotation;
        headsetZero = mainCamera.transform.localPosition;
        if (Global.global.rotationType == "relative")
        {
            playerModel.GetComponent<RotationConstraint>().constraintActive = true;
        }
        else if (Global.global.rotationType == "absolute")
        {
            playerModel.GetComponent<RotationConstraint>().constraintActive = false;
        }
        maxForwardLean = new Vector3(0, 0, 2.0f);
        maxRearwardLean = new Vector3(0, 0, -1.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //print("Headset Location: " + mainCamera.transform.localPosition);
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

        //set max forward lean
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            maxForwardLean = mainCamera.transform.localPosition;
        }

        //set max rearward lean
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            maxRearwardLean = mainCamera.transform.localPosition;
        }

        //space break
        if (Input.GetKey(KeyCode.B))
        {
            RB.velocity = Vector3.zero;
        }

        //get how much player is leaning and normalize
        float lean;
        float speedPercentage;
        if (mainCamera.transform.localPosition.z >= headsetZero.z)
        {
            lean = mainCamera.transform.localPosition.z / maxForwardLean.z;
            if (lean > 1.0f)
            {
                lean = 1.0f;
            }
            speedPercentage = targetSpeedCurve.Evaluate(lean);
        }
        else if (mainCamera.transform.localPosition.z < headsetZero.z)
        {
            lean = mainCamera.transform.localPosition.z / maxRearwardLean.z;
            if (lean > 1.0f)
            {
                lean = 1.0f;
            }
            speedPercentage = targetReverseSpeedCurve.Evaluate(lean);
        }
        else
        {
            speedPercentage = 0.0f;
        }

        //clamp max speed
        if (RB.velocity.magnitude > maxSpeed * speedPercentage)
        {
            RB.velocity = Vector3.ClampMagnitude(RB.velocity, maxSpeed * speedPercentage);
        }

        if (Global.global.rotationType == "relative")
        {
            RelativeRotateToCamera();
        }
        else if (Global.global.rotationType == "absolute")
        {
            AbsoluteRotateToCamera();
        }
        
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

    private void RelativeRotateToCamera()
    {
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, mainCamera.transform.rotation, Time.deltaTime * rotateSpeed);
    }
    private void AbsoluteRotateToCamera()
    {
        playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, mainCamera.transform.rotation, Time.deltaTime * rotateSpeed);
    }

    //Fire the lasers
    public void Shoot()
    {
        GenerateLaser("Prefabs/Laser", laserSpawner, laserSpeed, laserDamage);
        GenerateLaser("Prefabs/Laser", laserSpawner2, laserSpeed, laserDamage);
    }

    //Generate a single laser
    private void GenerateLaser(string prefabPath, GameObject laserSpawner, float laserSpeed, int laserDamage)
    {
        GameObject LaserInstance = Instantiate(Resources.Load<GameObject>(prefabPath), laserSpawner.transform.position, laserSpawner.transform.rotation) as GameObject;
        LaserInstance.GetComponent<LaserScript>().speed = RB.velocity.magnitude + laserSpeed;
        LaserInstance.GetComponent<LaserScript>().damage = laserDamage;
    }
}
