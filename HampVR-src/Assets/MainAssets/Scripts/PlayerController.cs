using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Valve.VR;

public class PlayerController : MonoBehaviour
{
    [Header("Debug")]
    [Tooltip("Should controller debug messages be on?")]
    public bool controllerDebug;
    [Tooltip("Should Movement debug messages be on?")]
    public bool movementDebug;

    [Header("GameObjects")]
    [Tooltip("The GameObject with the players RigidBody")]
    public GameObject playerPhysics;
    [Tooltip("The GameObject representing the ship detached from the cockpit.")]
    public GameObject playerModel;
    [Tooltip("The left controller.")]
    public GameObject leftHand;
    [Tooltip("The right controller.")]
    public GameObject rightHand;
    [Tooltip("The left laser spawner.")]
    public GameObject laserSpawner;
    [Tooltip("The right laser spawner.")]
    public GameObject laserSpawner2;
    [Tooltip("The camera being used as the headset.")]
    [SerializeField]
    private Camera mainCamera;
    
    private Rigidbody RB;

    [Header("Flight Model")]
    [Tooltip("The base acceleration value to be modified by the curve.")]
    public float acceleration;
    [Tooltip("How quickly the ship follows the players view.")]
    public float rotateSpeed;
    [Tooltip("The maximum speed achiveable by the player.")]
    public float maxSpeed;
    [Tooltip("THe area around the player where control inputs will not register in units.")]
    public float deadZone;
    [Tooltip("The mass of the ship. \n Overrides the rigidbody on the CameraRig.")]
    public float mass;
    [Tooltip("The drag of the ship (probably leave at 0). \n Overrides the rigidbody on the CameraRig.")]
    public float drag;
    [Tooltip("The angular drag of the ship (probably leave at 0). \n Overrides the rigidbody on the CameraRig.")]
    public float angularDrag;
    [Tooltip("The distance in units the player has to lean from the zero to get max speed.")]
    public float maxLean;

    [Header("Weapons/Health")]
    [Tooltip("The curve controlling how the ship achieves max speed throughout the players lean.")]
    public AnimationCurve targetSpeedCurve;

    public float laserSpeed;
    public int laserDamage;
    
    [Tooltip("The amount of health the player has.")]
    public int health;

    [Header("SteamVR")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean accelerate;
    public SteamVR_Action_Boolean deccelerate;
    public SteamVR_Action_Boolean fire;
    public SteamVR_Action_Boolean resetHeadsetZero;


    [SerializeField]
    [Tooltip("The vector of representing the force being applied to the ships rigidbody.")]
    private Vector3 horizontalMovementVector;

    private Vector3 headsetZero;
    

    [Header("Punching")]
    [Tooltip("Interval between samples of controller positions in seconds.")]
    public float handMoveLogTimerDefault;
    public int handMoveLogSize;
    private float handMoveLogTimer;
    [Tooltip("How far does a punch need to go to register.")]
    public float fireDistance;
    [Tooltip("How much time in handMoveLogTimer intervals the player has to punch fireDistance units.")]
    public int fireDetectionTime;
    private int fireDetectionIndex;
    private bool leftCocked;
    private bool rightCocked;
    private List<Vector3> leftLastPositions;
    private List<Vector3> rightLastPositions;




    // Start is called before the first frame update
    void Start()
    {
        RB = playerPhysics.GetComponent<Rigidbody>();
        RB.drag = drag;
        RB.angularDrag = angularDrag;
        RB.mass = mass;
        headsetZero = mainCamera.transform.localPosition;
        handMoveLogTimer = handMoveLogTimerDefault;
        for(int i = 0; i == handMoveLogSize-1; i++)
        {
            leftLastPositions[i] = Vector3.zero;
            rightLastPositions[i] = Vector3.zero;
        }

        leftCocked = false;
        rightCocked = false;
        if (Global.global.rotationType == "relative")
        {
            playerModel.GetComponent<RotationConstraint>().constraintActive = true;
        }
        else if (Global.global.rotationType == "absolute")
        {
            playerModel.GetComponent<RotationConstraint>().constraintActive = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetMovementVectors();

        ApplyForce();

        ControllerBehaviorHandler();

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
            if (movementDebug)
            {
                print("Speed percentage: " + speedPercentage + "%");
            }
        }
        else
        {
            speedPercentage = 0.0f;
        }

        //clamp max speed
        if (RB.velocity.magnitude > maxSpeed * speedPercentage)
        {
            RB.velocity = Vector3.ClampMagnitude(RB.velocity, maxSpeed * speedPercentage);
            if (movementDebug)
            {
                print("Velocity clamped to: " + RB.velocity);
            }
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
        //ControllerBehaviorHandler();

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

    //Generate a single laser
    private void GenerateLaser(string prefabPath, GameObject laserSpawner, Quaternion rotation, float laserSpeed, int laserDamage)
    {
        GameObject LaserInstance = Instantiate(Resources.Load<GameObject>(prefabPath), laserSpawner.transform.position, laserSpawner.transform.rotation) as GameObject;
        LaserInstance.transform.rotation = rotation;
        LaserInstance.GetComponent<LaserScript>().speed = RB.velocity.magnitude + laserSpeed;
        LaserInstance.GetComponent<LaserScript>().damage = laserDamage;
    }

    //Set the vector to control the players movement
    private void SetMovementVectors()
    {
        if ((mainCamera.transform.localPosition - headsetZero).magnitude > deadZone)
        {
            horizontalMovementVector = mainCamera.transform.localPosition - headsetZero;
            if (movementDebug)
            {
                print("Movement Vector: " + horizontalMovementVector);
            }
        }
        else
        {
            horizontalMovementVector = new Vector3(0, 0, 0);
        }
        
        horizontalMovementVector.y = 0;
        
        //verticalMarker.localPosition = new Vector3(verticalMarker.localPosition.x, verticalMarker.localPosition.y, horizontalMovementVector.magnitude);
        //verticalMovementVector.y = verticalMarker.position.y - headsetZero.y;
    }

    //Apply the vector to the player as a force
    private void ApplyForce()
    {
        if (mainCamera.transform.localPosition != headsetZero)
        {
            RB.AddForce(horizontalMovementVector.normalized * acceleration);

            if (movementDebug)
            {
                //Debug Raycast
                Ray horizontalMovementRay = new Ray(mainCamera.transform.position, mainCamera.transform.localPosition - headsetZero);
                Debug.DrawRay(horizontalMovementRay.origin, horizontalMovementRay.direction * 10, Color.red);
            } 
        }
    }


    //private void ControllerBehaviorHandler()
    //{
    //    Vector3 leftPosition = leftHand.transform.localPosition;
    //    Vector3 rightPosition = rightHand.transform.localPosition;

    //    handMoveLogTimer = handMoveLogTimer - Time.fixedUnscaledDeltaTime;




    //}



    //Track controller positions and determine whether to shoot.
    private void ControllerBehaviorHandler()
    {
        Vector3 leftPosition = leftHand.transform.localPosition;
        Vector3 rightPosition = rightHand.transform.localPosition;

        handMoveLogTimer = handMoveLogTimer - Time.fixedUnscaledDeltaTime;
        if (handMoveLogTimer <= 0)
        {
            if (controllerDebug)
            {
                print("Logging Left hand at " + leftPosition);
                print("Logging Right hand at " + rightPosition);
                print("leftLastPositions: " + leftLastPositions);
                print("rightLastPositions: " + rightLastPositions);
            }
            leftLastPositions.Insert(0, leftPosition);
            rightLastPositions.Insert(0, rightPosition);

            if (leftLastPositions.Count > handMoveLogSize)
            {
                for (int i = leftLastPositions.Count-1; i < handMoveLogSize - 1; i--)
                {
                    leftLastPositions.RemoveAt(i);
                }
            }

            if (rightLastPositions.Count > handMoveLogSize)
            {
                for (int i = rightLastPositions.Count-1; i < handMoveLogSize - 1; i--)
                {
                    rightLastPositions.RemoveAt(i);
                }
            }

            handMoveLogTimer = handMoveLogTimerDefault;
        }

        //if player has punched forward, then shoot
        if (GetFireDown())
        {
            if ((leftPosition - leftLastPositions[fireDetectionTime]).magnitude > fireDistance)
            {
                if (controllerDebug)
                {
                    print("Left Punch");
                }
                Quaternion leftWeaponRotation = new Quaternion();
                leftWeaponRotation.eulerAngles = leftPosition - leftLastPositions[fireDetectionTime];
                GenerateLaser("Prefabs/Laser", laserSpawner, leftWeaponRotation, laserSpeed, laserDamage);
            }
            if ((rightPosition - rightLastPositions[fireDetectionTime]).magnitude > fireDistance)
            {
                if (controllerDebug)
                {
                    print("Right Punch");
                }
                Quaternion rightWeaponRotation = new Quaternion();
                rightWeaponRotation.eulerAngles = leftPosition - leftLastPositions[fireDetectionTime];
                GenerateLaser("Prefabs/Laser", laserSpawner, rightWeaponRotation, laserSpeed, laserDamage);
            }
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
