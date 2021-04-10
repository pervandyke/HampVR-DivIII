using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour, IEnemy
{
    [Header("References")]
    public GameObject barrel;
    public GameObject pivot;
    public GameObject cap;
    public GameObject body;
    public GameObject laserEmitter;
    

    [Header("Weapons")]
    public float laserSpeed;
    [Tooltip("time in seconds between shots")]
    public float fireRate;
    public int damage;
    public float trackingSpeed;
    public float trackingDistance;
    public float allowedShotAngle;
    public bool playerInRange;
    private float shotTimer;

    private GameObject player;
    private Quaternion lookRotation;
    public bool allowedToShoot = true;

    [Header("General Stats")]
    public int health;
    


    // Start is called before the first frame update
    void Start()
    {
        
        shotTimer = fireRate;
        EnemyManager.enemyManager.AddEnemy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        TrackPlayer();
        //CheckShot(allowedShotAngle);
        Shoot();
    }

    private void TrackPlayer()
    {
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) < trackingDistance)
        {
            playerInRange = true;
            WhereToRotate(player.transform.position);

            //cap.transform.rotation = Quaternion.Slerp(cap.transform.rotation, lookRotation, Time.deltaTime * trackingSpeed);
            //cap.transform.rotation.eulerAngles.Set(0, cap.transform.rotation.eulerAngles.y, 0);

            pivot.transform.rotation = Quaternion.Slerp(pivot.transform.rotation, lookRotation, Time.deltaTime * trackingSpeed);
            //pivot.transform.rotation.eulerAngles.Set(pivot.transform.rotation.eulerAngles.x, 0, 0);
        }
        else
        {
            playerInRange = false;
        }
    }

    private void WhereToRotate(Vector3 target)
    {
        Vector3 aimPoint = CalculateLead();
        Vector3 direction = (aimPoint - pivot.transform.position).normalized;
        //print(gameObject + " should be aiming in direction " + direction);
        //Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + direction * 10, Color.red, Mathf.Infinity);
        lookRotation = Quaternion.LookRotation(direction);
    }

    private Vector3 CalculateLead(int iterations = 3, float leadScaleAdjustment = 1.0f)
    {
        float flightTime;
        Vector3 targetMovementPerSec;
        Vector3 estimatedHitPosition = Vector3.zero;
        for(int i = 0; i < iterations; i++)
        {
            if (i == 0)
            {
                flightTime = (player.transform.position - pivot.transform.position).magnitude / laserSpeed;
            }
            else
            {
                flightTime = (estimatedHitPosition - pivot.transform.position).magnitude / laserSpeed;
            }
            targetMovementPerSec = player.GetComponent<Rigidbody>().velocity;
            estimatedHitPosition = player.transform.position + (targetMovementPerSec * flightTime) * leadScaleAdjustment ;
        }
        //print(gameObject.name + " estimated hit position is " + estimatedHitPosition);
        return estimatedHitPosition;
    }

    private void CheckShot(float allowedAngle)
    {
        //print("Angle to player is: " + Vector3.Angle(barrel.transform.position, player.transform.position));
        if (Vector3.Angle(barrel.transform.position, player.transform.position) < allowedAngle)
        {
            allowedToShoot = true;
        }
        else
        {
            allowedToShoot = false;
        }
    }

    private void Shoot()
    {
        shotTimer -= Time.deltaTime;
        if (playerInRange && shotTimer <= 0.0f)
        {
            GameObject shotInstance = Instantiate(Resources.Load("Prefabs/Laser"), laserEmitter.transform.position, laserEmitter.transform.rotation) as GameObject;
            shotInstance.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, laserSpeed);
            shotInstance.GetComponent<LaserScript>().damage = damage;
            shotTimer = fireRate;
            //print("pew");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        print("applied damage");
        if (health <= 0)
        {
            if(Global.global.leftSelectedTarget == gameObject)
            {
                Global.global.leftSelectedTarget = null;
            }
            else if (Global.global.rightSelectedTarget == gameObject)
            {
                Global.global.rightSelectedTarget = null;
            }
            EnemyManager.enemyManager.RemoveEnemy(gameObject);
            Instantiate(Resources.Load("Prefabs/explosion1"), gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}
