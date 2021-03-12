using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Valve.VR;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerController;

    [Header("Debug")]
    [Tooltip("Should controller debug messages be on?")]
    public bool controllerDebug;
    [Tooltip("Should Weapon debug messages be on?")]
    public bool weaponDebug;
    [Tooltip("Should Movement debug messages be on?")]
    public bool movementDebug;
    [Tooltip("Should Shield debug messages be on?")]
    public bool shieldDebug;
    [Tooltip("Should Selection Sphere be visible?")]
    public bool selectionDebug;

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
    public Camera mainCamera;
    
    public Rigidbody RB;

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
    public float missileTurningSpeed;
    
    [Tooltip("The amount of health the player has.")]
    public int health;

    private bool leftWeaponCooldown = false;
    private bool rightWeaponCooldown = false;
    private bool leftShieldCooldown = false;
    private bool rightShieldCooldown = false;

    [Header("SteamVR")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean accelerate;
    public SteamVR_Action_Boolean deccelerate;
    public SteamVR_Action_Boolean leftFire;
    public SteamVR_Action_Boolean rightFire;
    public SteamVR_Action_Boolean resetHeadsetZero;
    public SteamVR_Action_Boolean leftSelect;
    public SteamVR_Action_Boolean rightSelect;


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

    public float missileCooldownDefault;
    private float leftCooldownTimer;
    private float rightCooldownTimer;

    [Header("Shields")]
    [Tooltip("The maximum value of the left shield.")]
    public int leftShieldMax;
    private int leftShieldValue;
    [Tooltip("The maximum value of the right shield.")]
    public int rightShieldMax;
    private int rightShieldValue;
    [Tooltip("How far does a slap need to go to register.")]
    public float slapDistance;
    [Tooltip("How much time in handMoveLogTimer intervals the player has to slap slapDistance units.")]
    public int slapDetectionTime;
    [Tooltip("How long shield boost should last.")]
    public float shieldBoostTime;
    [Tooltip("How long shield should drop after boosting.")]
    public float shieldCooldownTime;
    private float leftBoostTimer;
    private float rightBoostTimer;
    private float leftBoostCooldownTimer;
    private float rightBoostCooldownTimer;
    private bool leftShieldBoosted;
    private bool rightShieldBoosted;

    [Header("Selection")]

    public float viewAngleMin = 0.0f;
    public float viewAngleMax = 110.0f;
    public float viewAngleWeight;
    public float distanceMin = 0.0f;
    public float distanceMax;
    public float distanceWeight;

    private List<Vector3> leftLastPositions;
    private List<Vector3> rightLastPositions;
    private bool leftSelecting = false;
    private bool rightSelecting = false;
    private List<Vector3> leftSelectionPoints;
    private List<Vector3> rightSelectionPoints;

    [Header("Misc")]
    public LayerMask cockpitMask;


    // Start is called before the first frame update
    void Start()
    {
        RB = playerPhysics.GetComponent<Rigidbody>();
        RB.drag = drag;
        RB.angularDrag = angularDrag;
        RB.mass = mass;
        headsetZero = mainCamera.transform.localPosition;
        handMoveLogTimer = handMoveLogTimerDefault;
        leftLastPositions = new List<Vector3>();
        rightLastPositions = new List<Vector3>();
        leftSelectionPoints = new List<Vector3>();
        rightSelectionPoints = new List<Vector3>();
        leftShieldValue = leftShieldMax;
        rightShieldValue = rightShieldMax;
        leftBoostTimer = shieldBoostTime;
        rightBoostTimer = shieldBoostTime;
        leftBoostCooldownTimer = 0.0f;
        rightBoostCooldownTimer = 0.0f;
        leftCooldownTimer = missileCooldownDefault;
        rightCooldownTimer = missileCooldownDefault;
        for (int i = 0; i == handMoveLogSize-1; i++)
        {
            leftLastPositions[i] = Vector3.zero;
            rightLastPositions[i] = Vector3.zero;
        }
    }


    void FixedUpdate()
    {
        horizontalMovementVector = SetMovementVectors();

        ApplyForce();

        ControllerBehaviorHandler();

        if (leftWeaponCooldown)
        {
            leftCooldownTimer -= Time.fixedDeltaTime;
            print("left cooldown: " + leftCooldownTimer);
            if (leftCooldownTimer <= 0)
            {
                leftCooldownTimer = missileCooldownDefault;
                leftWeaponCooldown = false;
            }
        }

        if (rightWeaponCooldown)
        {
            rightCooldownTimer -= Time.fixedDeltaTime;
            print("right cooldown: " + rightCooldownTimer);
            if (rightCooldownTimer <= 0)
            {
                rightCooldownTimer = missileCooldownDefault;
                rightWeaponCooldown = false;
            }
        }

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
        if (health <= 0)
        {
            //opaque canopy and change to state 2
        }
    }

    private void RelativeRotateToCamera()
    {
        playerPhysics.transform.rotation = Quaternion.Slerp(playerPhysics.transform.rotation, mainCamera.transform.rotation, Time.deltaTime * rotateSpeed);
    }
    private void AbsoluteRotateToCamera()
    {
        playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, mainCamera.transform.rotation, Time.deltaTime * rotateSpeed);
        playerModel.transform.rotation = Quaternion.Euler(0,playerModel.transform.rotation.eulerAngles.y,0);
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

        PunchCheck(leftPosition, rightPosition);

        SelectionButtonHeld();

        //SlapCheck(leftPosition, rightPosition);
    }

    private void SlapCheck(Vector3 leftPosition, Vector3 rightPosition)
    {
        /*
         * Player rapidly moves hand while not holding fire button
         * boost sheild in that quarter
         * then after a tunable amount of time, drop the sheild for a tunable amount of time
         * if both hands slap at the same time, sheild blast attack
         */

        if (true)  // put button here if we need one
        {
            //left slap detection
            if ((leftPosition - leftLastPositions[slapDetectionTime]).magnitude > slapDistance && leftShieldCooldown == false)
            {
                if (shieldDebug)
                {
                    print("Left Slap");
                    print("Left Position: " + leftPosition + "\nLeft Last Position: " + leftLastPositions[slapDetectionTime]);
                }
                BoostShield("left");
            }

            //left timers
            if (leftBoostTimer <= 0 && leftShieldBoosted == true)
            {
                leftShieldBoosted = false;
                leftShieldCooldown = true;
                leftBoostTimer = shieldBoostTime;
            }
            else if (leftBoostTimer > 0 && leftShieldBoosted == true)
            {
                leftBoostTimer = leftBoostTimer - Time.fixedDeltaTime;
            }

            if (leftBoostCooldownTimer <= 0 && leftShieldCooldown == true)
            {
                leftShieldCooldown = false;
                leftBoostCooldownTimer = shieldCooldownTime;
            }
            else if (leftBoostCooldownTimer > 0 && leftShieldCooldown == true)
            {
                leftBoostCooldownTimer = leftBoostCooldownTimer - Time.fixedDeltaTime;
            }

            //right slap detection
            if ((rightPosition - rightLastPositions[slapDetectionTime]).magnitude > slapDistance && rightShieldCooldown == false)
            {
                if (shieldDebug)
                {
                    print("Right Slap");
                    print("Right Position: " + rightPosition + "\nRight Last Position: " + rightLastPositions[slapDetectionTime]);
                }
                BoostShield("right");
            }

            //right timers
            if (rightBoostTimer <= 0 && rightShieldBoosted == true)
            {
                rightShieldBoosted = false;
                rightShieldCooldown = true;
                rightBoostTimer = shieldBoostTime;
            }
            else if (rightBoostTimer > 0 && rightShieldBoosted == true)
            {
                rightBoostTimer = rightBoostTimer - Time.fixedDeltaTime;
            }

            if (rightBoostCooldownTimer <= 0 && rightShieldCooldown == true)
            {
                rightShieldCooldown = false;
                rightBoostCooldownTimer = shieldCooldownTime;
            }
            else if (rightBoostCooldownTimer > 0 && rightShieldCooldown == true)
            {
                rightBoostCooldownTimer = rightBoostCooldownTimer - Time.fixedDeltaTime;
            }
        }

    }

    private void SelectionButtonHeld()
    {
        if (GetLeftSelect())
        {
            if (!leftSelecting)
            {
                leftSelecting = true;
            }
            leftSelectionPoints.Add(leftHand.transform.localPosition);
        }
        else if (!GetLeftSelect() && leftSelecting)
        {
            leftSelecting = false;
            List<GameObject> validSelections = RunSelectionAlgorithm(leftSelectionPoints);
            if(validSelections.Count > 0)
            {
                GameObject objectToSelect = RunSelectionPriorityAlgorithm(validSelections);
                if (Global.global.leftSelectedTarget != null)
                {
                    //Global.global.leftSelectedTarget.GetComponent<MeshRenderer>().material.color = Color.white;
                }
                Global.global.leftSelectedTarget = objectToSelect;
            }
        }

        if (GetRightSelect())
        {
            if (!rightSelecting)
            {
                rightSelecting = true;
            }
            rightSelectionPoints.Add(rightHand.transform.localPosition);
        }
        else if (!GetRightSelect() && rightSelecting)
        {
            rightSelecting = false;
            List<GameObject> validSelections = RunSelectionAlgorithm(rightSelectionPoints);
            if (validSelections.Count > 0)
            {
                GameObject objectToSelect = RunSelectionPriorityAlgorithm(validSelections);
                if (Global.global.rightSelectedTarget != null)
                {
                    //Global.global.rightSelectedTarget.GetComponent<MeshRenderer>().material.color = Color.white;
                }
                Global.global.rightSelectedTarget = objectToSelect;
            }
        }
    }

    

    private List<GameObject> RunSelectionAlgorithm(List<Vector3> selectionPoints)
    {
        //find the average point of the circle
        Vector3 averagePoint = Vector3.zero;
        foreach (Vector3 point in selectionPoints)
        {
            averagePoint = averagePoint + point;
        }
        averagePoint = averagePoint / selectionPoints.Count;
        //find the point farthest from the average
        float farthestPointDistance = 0;
        foreach (Vector3 point in selectionPoints)
        {
            float distance = Vector3.Distance(point, averagePoint);
            if (distance > farthestPointDistance)
            {
                farthestPointDistance = distance;
            }
        }
        //create a sphere with a diameter == to twice the distance from the average point fo the furthest point
        GameObject selectionSphere = Instantiate(Resources.Load("Prefabs/SelectionSphere")) as GameObject;
        selectionSphere.transform.parent = playerPhysics.transform;
        if (!selectionDebug)
        {
            selectionSphere.GetComponent<MeshRenderer>().enabled = false;
        }
        selectionSphere.transform.localPosition = averagePoint;
        selectionSphere.transform.localScale = new Vector3(farthestPointDistance * 2, farthestPointDistance * 2, farthestPointDistance * 2);
        selectionSphere.transform.parent = null;
        selectionPoints.Clear();

        //find every enemy that would be a valid selection
        List<GameObject> validSelections = new List<GameObject>();
        foreach (GameObject enemy in EnemyManager.enemyManager.enemies)
        {
            //cast a ray from enemy to headset, if it hits the selection sphere add it to the list
            Vector3 direction = (mainCamera.transform.position - enemy.transform.position).normalized;
            RaycastHit hitData;
            bool didHit = Physics.Raycast(enemy.transform.position, direction, out hitData, Vector3.Distance(enemy.transform.position, mainCamera.transform.position),
                cockpitMask, QueryTriggerInteraction.Collide);
            
            if (selectionDebug)
            {
                print("Drawing Ray from enemy " + enemy.name + " Along direction " + direction);
                Ray selectionRay = new Ray(enemy.transform.position, direction);
                Color rayColor;
                if (!didHit)
                {
                    rayColor = Color.yellow;
                }
                else if (didHit && hitData.collider.gameObject.tag == "SelectionSphere")
                {
                    rayColor = Color.red;
                }
                else
                {
                    rayColor = Color.green;
                }
                Debug.DrawRay(selectionRay.origin, selectionRay.direction * Vector3.Distance(mainCamera.transform.position, enemy.transform.position), rayColor, 10.0f);
            }

            if (didHit)
            {
                if (selectionDebug)
                {
                    print("Ray hit: " + hitData.collider.gameObject.name);
                }

                if (hitData.collider.gameObject.tag == "SelectionSphere")
                {
                    validSelections.Add(enemy);
                }
            }
        }
        return validSelections;
    }

    private GameObject RunSelectionPriorityAlgorithm(List<GameObject> validSelections)
    {
        GameObject objectToSelect;
        objectToSelect = validSelections[0];
        List<TargetSelectionValues> validSelectionValues = new List<TargetSelectionValues>();
        if (validSelections.Count > 1)
        {
            //for each valid selection
            for (int i = 0; i < validSelections.Count; i++)
            {
                //create TargetSelectionValue entry
                validSelectionValues.Add(new TargetSelectionValues() {potentialTarget = validSelections[i]});

                //get the angle to player view
                Vector3 enemyDirection = validSelections[i].transform.position - mainCamera.transform.position;
                float viewAngle = Vector3.Angle(enemyDirection, mainCamera.transform.forward);
                validSelectionValues[i].normalizedAngleToView = Mathf.InverseLerp(viewAngleMax, viewAngleMin, viewAngle);

                //get the angle to the forward direction of the selection sphere

                //get the distance to target
                float distanceToSelectionCandidate = Vector3.Distance(mainCamera.transform.position, validSelectionValues[i].potentialTarget.transform.position);
                validSelectionValues[i].normalizedDistance = Mathf.InverseLerp(distanceMax, distanceMin, distanceToSelectionCandidate);
                //get the target threat value

                //Multiply all the normalized values by multipliers and combine (currently addition)
                validSelectionValues[i].selectionValue = (validSelectionValues[i].normalizedAngleToView * viewAngleWeight) + (validSelectionValues[i].normalizedDistance * distanceWeight);
            }

            //select the target with the highest SelectionValue
            TargetSelectionValues currentTargetSelectionValues = validSelectionValues[0];
            for (int i = 0; i < validSelectionValues.Count; i++)
            {
                if (currentTargetSelectionValues.selectionValue < validSelectionValues[i].selectionValue)
                {
                    currentTargetSelectionValues = validSelectionValues[i]; 
                }
            }
            objectToSelect = currentTargetSelectionValues.potentialTarget;
            
        }
        //objectToSelect.GetComponent<MeshRenderer>().material.color = Color.red;
        return objectToSelect;
    }

   
    private void PunchCheck(Vector3 leftPosition, Vector3 rightPosition)
    {
        //if player has punched forward, then shoot
        if (GetLeftFireDown())
        {
            if ((leftPosition - leftLastPositions[fireDetectionTime]).magnitude > fireDistance && !leftWeaponCooldown)
            {
                if (weaponDebug)
                {
                    print("Left Punch");
                    print("Left Position: " + leftPosition + "\nLeft Last Position: " + leftLastPositions[fireDetectionTime]);
                }

                WeaponsLibrary.wepLib.FireLongRangeMissile(laserSpawner2, RB, laserSpawner2.transform.rotation, laserSpeed, missileTurningSpeed, laserDamage, Global.global.leftSelectedTarget, SteamVR_Input_Sources.LeftHand);
                leftWeaponCooldown = true;
            }
        }

        

        if (GetRightFireDown())
        {
            if ((rightPosition - rightLastPositions[fireDetectionTime]).magnitude > fireDistance && !rightWeaponCooldown)
            {
                if (weaponDebug)
                {
                    print("Right Punch");
                    print("Right Position: " + rightPosition + "\nRight Last Position: " + rightLastPositions[fireDetectionTime]);
                }

                WeaponsLibrary.wepLib.FireLongRangeMissile(laserSpawner, RB, laserSpawner.transform.rotation, laserSpeed, missileTurningSpeed, laserDamage, Global.global.rightSelectedTarget, SteamVR_Input_Sources.RightHand);
                rightWeaponCooldown = true;
            }
        }

       
    }

    private void BoostShield(string shield)
    {
        if (shield == "left")
        {
            leftShieldBoosted = true;
        }
        else if (shield == "right")
        {
            rightShieldBoosted = true;
        }
    }

    public void TakeHit(int damage, GameObject projectile)
    {
        Vector3 hitDirection = projectile.transform.position - transform.position;
        float collisionAngle = Vector3.SignedAngle(hitDirection, transform.forward, Vector3.up);
        if (collisionAngle > 0)
        {
            rightShieldValue = TakeDamage(rightShieldBoosted, damage, leftShieldValue);
        }
        else if (collisionAngle < 0)
        {
            leftShieldValue = TakeDamage(leftShieldBoosted, damage, leftShieldValue);
        }
    }

    private int TakeDamage(bool shieldBoost, int damage, int shieldValue)
    {
        if (shieldBoost)
        {
            //no damage
        }
        else
        {
            int damageOverflow = damage - shieldValue;
            if (damageOverflow > 0 && shieldValue > 0)
            {
                shieldValue = 0;
            }
            else if (shieldValue == 0)
            {
                health = health - damageOverflow;
            }
            else
            {
                shieldValue = shieldValue - damage;
            }
        }

        return shieldValue;
    }

    public bool GetAccelerateDown()
    {
        return accelerate.GetState(handType);
    }

    public bool GetDeccelerateDown()
    {
        return deccelerate.GetState(handType);
    }

    public bool GetLeftFireDown()
    {
        return leftFire.GetState(handType);
    }

    public bool GetRightFireDown()
    {
        return rightFire.GetState(handType);
    }

    public bool GetResetHeadsetDown()
    {
        return resetHeadsetZero.GetStateDown(handType);
    }

    public bool GetLeftSelect()
    {
        return leftSelect.GetState(handType);
    }

    public bool GetRightSelect()
    {
        return rightSelect.GetState(handType);
    }

}

public class TargetSelectionValues
{
    public GameObject potentialTarget;

    public float normalizedDistance;

    public float normalizedAngleToView;

    public float normalizedAngleToSphere;

    public float normalizedThreatRating;

    public float selectionValue; //higher is better
}