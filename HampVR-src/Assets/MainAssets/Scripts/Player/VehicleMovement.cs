using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Valve.VR;

public class VehicleMovement : MonoBehaviour
{
    public static VehicleMovement vehicleMovement;

    #region tunable variables
    [Header("Debug")]
    [Tooltip("Should Movement debug messages be on?")]
    public bool movementDebug;

    [Header("GameObjects")]
    [Tooltip("The GameObject with the players RigidBody")]
    public GameObject playerPhysics;
    [Tooltip("The GameObject representing the ship detached from the cockpit.")]
    public GameObject playerModel;

    [Tooltip("The camera being used as the headset.")]
    public Camera mainCamera;

    public Rigidbody RB;

    [Header("Flight Model")]
    [Tooltip("The base acceleration value to be modified by the curve.")]
    public float acceleration;
    [Tooltip("How quickly the ship follows the players view.")]
    public float rotateSpeed;
    [Tooltip("The maximum speed achiveable by the player.")]
    public float maxSpeed;
    [Tooltip("The area around the player where control inputs will not register in units.")]
    public float deadZone;
    [Tooltip("The mass of the ship. \n Overrides the rigidbody on the CameraRig.")]
    public float mass;
    [Tooltip("The drag of the ship (probably leave at 0). \n Overrides the rigidbody on the CameraRig.")]
    public float drag;
    [Tooltip("The angular drag of the ship (probably leave at 0). \n Overrides the rigidbody on the CameraRig.")]
    public float angularDrag;
    [Tooltip("The distance in units the player has to lean from the zero to get max speed.")]
    public float maxLean;
    [Tooltip("The curve controlling how the ship achieves max speed throughout the players lean.")]
    public AnimationCurve targetSpeedCurve;

    [Header("SteamVR")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean resetHeadsetZero;

    [SerializeField]
    [Tooltip("The vector of representing the force being applied to the ships rigidbody.")]
    private Vector3 horizontalMovementVector;

    private Vector3 headsetZero;

    [Header("Misc")]
    public LayerMask cockpitMask;
    public UIParentScript uiParentScript; //include the ui parent script as a variable in this script so that we can call the height adjustment function
    #endregion tunable variables


    private void Awake()
    {
        vehicleMovement = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //set rigidbody tunables to the values defined in the editor
        RB = playerPhysics.GetComponent<Rigidbody>();
        RB.drag = drag;
        RB.angularDrag = angularDrag;
        RB.mass = mass;

        headsetZero = mainCamera.transform.localPosition; //replace this with a new ResetControlSpace()
    }

    void FixedUpdate()
    {
        //Reset Control Zero
        if (GetResetHeadsetDown())
        {
            headsetZero = mainCamera.transform.localPosition; //set the control space center to the current headset location
            uiParentScript.ChangeUIHeight(headsetZero); //update ui height

        }

        if(LevelState.levelState.levelStatus == 1)
        {
            horizontalMovementVector = SetMovementVectors();

            ApplyForce();

            //
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
                    print("Lean: " + lean);
                    print("Speed percentage: " + speedPercentage + "%");
                    print("Target Speed: " + maxSpeed * speedPercentage);
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
        }

        if (Global.global.rotationType == "absolute")
        {
            AbsoluteRotateToCamera();
        }
    }

    private void AbsoluteRotateToCamera()
    {
        //set rotation
        playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, mainCamera.transform.rotation, Time.deltaTime * rotateSpeed);
        //zero out x and z rotations
        playerModel.transform.rotation = Quaternion.Euler(0, playerModel.transform.rotation.eulerAngles.y, 0);
    }

    //Set the vector to control the players movement
    private Vector3 SetMovementVectors()
    {
        Vector3 localHorizontalMovementVector = Vector3.zero;
        if ((mainCamera.transform.localPosition - headsetZero).magnitude > deadZone)
        {
            localHorizontalMovementVector = mainCamera.transform.localPosition - headsetZero;
            if (movementDebug)
            {
                print("Movement Vector: " + horizontalMovementVector);
            }
        }

        localHorizontalMovementVector.y = 0;
        return localHorizontalMovementVector;
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

    public bool GetResetHeadsetDown()
    {
        return resetHeadsetZero.GetStateDown(handType);
    }

    public Quaternion HeadsetRotation2d()
    {
        return Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);
    }
}
