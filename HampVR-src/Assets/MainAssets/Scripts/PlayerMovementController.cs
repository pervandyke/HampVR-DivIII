using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Valve.VR;

public class PlayerMovementController : MonoBehaviour
{

    public GameObject playerPhysics;
    public GameObject playerModel;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject laserSpawner;
    public GameObject laserSpawner2;
    private Rigidbody RB;

    public float acceleration;
    public float rotateSpeed;
    public float maxSpeed;
    public float deadZone;

    public AnimationCurve targetSpeedCurve;

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
    private Vector3 horizontalMovementVector;

    private Vector3 headsetZero;
    private float maxLean;

    public float handMoveLogTimerDefault;
    private float handMoveLogTimer;
    public float fireDistance;
    public int fireDetectionTime;
    private bool leftCocked;
    private bool rightCocked;
    private List<Vector3> leftLastPositions;
    private List<Vector3> rightLastPositions;




    // Start is called before the first frame update
    void Start()
    {
        RB = playerPhysics.GetComponent<Rigidbody>();
        headsetZero = mainCamera.transform.localPosition;
        handMoveLogTimer = handMoveLogTimerDefault;
        for(int i = 0; i == 9; i++)
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
        maxLean = 0.75f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetMovementVectors();

        ApplyForce();

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
        ControllerBehaviorHandler();

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
    private void GenerateLaser(string prefabPath, GameObject laserSpawner, float laserSpeed, int laserDamage)
    {
        GameObject LaserInstance = Instantiate(Resources.Load<GameObject>(prefabPath), laserSpawner.transform.position, laserSpawner.transform.rotation) as GameObject;
        LaserInstance.GetComponent<LaserScript>().speed = RB.velocity.magnitude + laserSpeed;
        LaserInstance.GetComponent<LaserScript>().damage = laserDamage;
    }

    //Set the vector to control the players movement
    private void SetMovementVectors()
    {
        if ((mainCamera.transform.localPosition - headsetZero).magnitude > deadZone)
        {
            horizontalMovementVector = mainCamera.transform.localPosition - headsetZero;
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
            //RB.AddForce(verticalMovementVector.normalized * acceleration);

            //Debug Raycast
            Ray horizontalMovementRay = new Ray(mainCamera.transform.position, mainCamera.transform.localPosition - headsetZero);
            //Ray verticalMovementRay = new Ray(mainCamera.transform.position, Vector3.up);
            Debug.DrawRay(horizontalMovementRay.origin, horizontalMovementRay.direction * 10, Color.red);
            //Debug.DrawRay(verticalMovementRay.origin, verticalMovementRay.direction * verticalMovementVector.magnitude, Color.yellow);
        }
    }

    //Track controller positions and determine whether to shoot.
    //Currently doesn't account for facing of player, so is actually broken
    private void ControllerBehaviorHandler()
    {
        Vector3 leftPosition = leftHand.transform.localPosition;
        Vector3 rightPosition = rightHand.transform.localPosition;

        handMoveLogTimer = handMoveLogTimer - Time.deltaTime;
        if (handMoveLogTimer <= 0)
        {
            leftLastPositions.Add(leftPosition);
            rightLastPositions.Add(rightPosition);

            if (leftLastPositions.Count > 10)
            {
                for (int i = leftLastPositions.Count-1; i <= 9; i--)
                {
                    leftLastPositions.RemoveAt(i);
                }
            }

            if (rightLastPositions.Count > 10)
            {
                for (int i = rightLastPositions.Count-1; i <= 9; i--)
                {
                    rightLastPositions.RemoveAt(i);
                }
            }

        }

        //if player has punched forward, then shoot
        if (GetFireDown())
        {
            if ((leftPosition - leftLastPositions[fireDetectionTime]).magnitude > fireDistance)
            {
                GenerateLaser("Prefabs/Laser", laserSpawner, laserSpeed, laserDamage);
            }
            if ((rightPosition - rightLastPositions[fireDetectionTime]).magnitude > fireDistance)
            {
                GenerateLaser("Prefabs/Laser", laserSpawner2, laserSpeed, laserDamage);
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
