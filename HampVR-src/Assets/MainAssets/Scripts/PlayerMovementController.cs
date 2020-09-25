using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Valve.VR;

public class PlayerMovementController : MonoBehaviour
{

    public GameObject playerPhysics;
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
    [SerializeField]
    private Transform verticalMarker;
    [SerializeField]
    private Vector3 horizontalMovementVector;
    [SerializeField]
    private Vector3 verticalMovementVector;

    private Vector3 headsetZero;
    private float maxLean;




    // Start is called before the first frame update
    void Start()
    {
        RB = playerPhysics.GetComponent<Rigidbody>();
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
        maxLean = 0.75f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //print("Headset Location: " + mainCamera.transform.localPosition);
        //Acceleration
        /*
        print(mainCamera.transform.localPosition.z - headsetZero.z);
        if (Input.GetKey(KeyCode.LeftShift) || GetAccelerateDown() || mainCamera.transform.localPosition.z > headsetZero.z)
        {
            print("forward");
            RB.AddRelativeForce(Vector3.forward * acceleration);
        }
        else if (Input.GetKey(KeyCode.LeftControl) || GetDeccelerateDown() || mainCamera.transform.localPosition.z < headsetZero.z)
        {
            RB.AddRelativeForce(Vector3.forward * decceleration);
        }
        */

        //New Combined Acceleration

        SetMovementVectors();

        if (mainCamera.transform.localPosition != headsetZero)
        {
            RB.AddForce(horizontalMovementVector.normalized * acceleration);
            RB.AddForce(verticalMovementVector.normalized * acceleration);
            
            //Debug Raycast
            Ray horizontalMovementRay = new Ray(mainCamera.transform.position, mainCamera.transform.localPosition - headsetZero);
            Ray verticalMovementRay = new Ray(mainCamera.transform.position, Vector3.up);
            Debug.DrawRay(horizontalMovementRay.origin, horizontalMovementRay.direction * 10, Color.red);
            Debug.DrawRay(verticalMovementRay.origin, verticalMovementRay.direction * verticalMovementVector.magnitude, Color.yellow);
        }

        //Temporarily disabled straifing until we can tune it better

        /*
        //Strafe Up/Down
        if (Input.GetKey(KeyCode.Space) || mainCamera.transform.localPosition.y > headsetZero.y)
        {
            RB.AddRelativeForce(Vector3.up * strafeSpeed);
        }
        else if (Input.GetKey(KeyCode.C) || mainCamera.transform.localPosition.y < headsetZero.y)
        {
            RB.AddRelativeForce(Vector3.up * -strafeSpeed);
        }
        */

        /*
        //Strafe Left/Right
        if (Input.GetKey(KeyCode.Z) || mainCamera.transform.localPosition.x < headsetZero.x)
        {
            RB.AddRelativeForce(Vector3.right * -strafeSpeed);
        }
        else if (Input.GetKey(KeyCode.X) || mainCamera.transform.localPosition.x > headsetZero.x)
        {
            RB.AddRelativeForce(Vector3.right * strafeSpeed);
        }
        */

        PitchRollYawKeyboard();
       
        //Reset Control Zero
        if (GetResetHeadsetDown())
        {
            headsetZero = mainCamera.transform.localPosition;
        }

        //set max forward lean
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            maxLean = mainCamera.transform.localPosition.z;
        }

        //space break
        if (Input.GetKey(KeyCode.B))
        {
            RB.velocity = Vector3.zero;
        }

        /*
        //get how much player is leaning and normalize
        float lean;
        float speedPercentage;
        if (mainCamera.transform.localPosition.z >= headsetZero.z)
        {
            print("leaning forward");
            lean = mainCamera.transform.localPosition.z - headsetZero.z / maxForwardLean.z;
            if (lean > 1.0f)
            {
                lean = 1.0f;
            }
            speedPercentage = targetSpeedCurve.Evaluate(lean);
        }
        else if (mainCamera.transform.localPosition.z < headsetZero.z)
        {
            lean = mainCamera.transform.localPosition.z - headsetZero.z / maxRearwardLean.z;
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
        */

        float lean;
        float speedPercentage;
        if (mainCamera.transform.localPosition.z != headsetZero.z)
        {
            lean = horizontalMovementVector.magnitude / maxLean;
            if (lean > 1.0f)
            {
                lean = 1.0f;
            }
            speedPercentage = targetSpeedCurve.Evaluate(lean);
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

    
    private void Update()
    {
        //Shooting
        if (Input.GetKeyDown(KeyCode.F) || GetFireDown())
        {
            Shoot();
        }

        if (health <= 0)
        {
            Destroy(playerPhysics);
        }
    }

    private void RelativeRotateToCamera()
    {
        playerPhysics.transform.rotation = Quaternion.Slerp(playerPhysics.transform.rotation, mainCamera.transform.rotation, Time.deltaTime * rotateSpeed);
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

    private void SetMovementVectors()
    {
        horizontalMovementVector = mainCamera.transform.localPosition - headsetZero;
        horizontalMovementVector.y = 0;
        
        verticalMarker.localPosition = new Vector3(verticalMarker.localPosition.x, verticalMarker.localPosition.y, horizontalMovementVector.magnitude);
        verticalMovementVector.y = verticalMarker.position.y - headsetZero.y;
    }

    private void PitchRollYawKeyboard()
    {
        //Pitch, Roll, and Yaw are all keyboard only for now, all rotation handled with headset input

        //Pitch
        if (Input.GetKey(KeyCode.W))
        {
            playerPhysics.transform.Rotate(new Vector3(rotateSpeed, 0, 0));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerPhysics.transform.Rotate(new Vector3(-rotateSpeed, 0, 0));
        }

        //Roll
        if (Input.GetKey(KeyCode.Q))
        {
            playerPhysics.transform.Rotate(new Vector3(0, 0, rotateSpeed));
        }
        else if (Input.GetKey(KeyCode.E))
        {
            playerPhysics.transform.Rotate(new Vector3(0, 0, -rotateSpeed));
        }

        //Yaw
        if (Input.GetKey(KeyCode.D))
        {
            playerPhysics.transform.Rotate(new Vector3(0, rotateSpeed, 0));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            playerPhysics.transform.Rotate(new Vector3(0, -rotateSpeed, 0));
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

}
