using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_V1 : MonoBehaviour
{
    public GameObject laserSpawner;
    private Rigidbody RB;
    public GameObject target;
    private Vector3 direction;
    private Quaternion lookRotation;

    public float speed;
    public float acceleration;
    public float decceleration;
    public float strafeSpeed;
    public float rotateSpeed;

    public float laserSpeed;
    public float laserTimerDefault;
    public int laserDamage;
    public int health;
    
    private float laserTimer;

    

    // Start is called before the first frame update
    void Start()
    {
        RB = gameObject.GetComponent<Rigidbody>();
        laserTimer = laserTimerDefault;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        WhereToRotate(target.transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        DecisionTree();
        if (RB.velocity.magnitude > speed)
        {
            RB.velocity = Vector3.ClampMagnitude(RB.velocity, speed);
        }
    }

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void WhereToRotate(Vector3 target)
    {
        direction = (target - transform.position).normalized;
        lookRotation = Quaternion.LookRotation(direction);
    }
    private void DecisionTree()
    {
        if (DistanceToTarget(target) < 5)
        {
            RB.velocity = transform.forward * decceleration;
        }
        else
        {
            RB.velocity = transform.forward * speed;
        }

        /*else
        {
            if (RB.velocity.magnitude > 0)
            {
                RB.velocity = RB.velocity + (transform.forward * decceleration);
            }
            else if (RB.velocity.magnitude <= 0)
            {
                RB.velocity = Vector3.zero;
            }
        }*/

        if (DistanceToTarget(target) < 40)
        {
            laserTimer -= Time.deltaTime;
            RaycastHit forwardHit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out forwardHit, 40.0f) && laserTimer <= 0)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward),Color.white, 40.0f);
                Shoot();
                laserTimer = laserTimerDefault;
                print("Raycast hit");
                if (forwardHit.transform.gameObject.layer == 10 && forwardHit.transform.gameObject.tag == "Player")
                {
                    print("shooting");
                    
                    
                }
            }
        }
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 10 && other.gameObject.tag == "Enemy")
        {
            Vector3 otherPosition = other.ClosestPoint(gameObject.transform.position);
            Vector3 positionDifference = (transform.position - otherPosition).normalized;
            RB.AddForce(positionDifference * strafeSpeed);
        }
    }*/

    public void DoDamage(int damage)
    {
        health -= damage;
        print("applied damage");
    }

    private float DistanceToTarget(GameObject target)
    {
        Vector3 tp = target.transform.position;
        Vector3 p = transform.position;
        float distance = Vector3.Distance(tp, p);

        return (distance);
    }
    public void Shoot()
    {
        Vector3 SpawnPosition = laserSpawner.transform.position;
        Quaternion SpawnRotation = laserSpawner.transform.rotation;
        GameObject LaserInstance;
        LaserInstance = Instantiate(Resources.Load<GameObject>("Prefabs/Laser"), SpawnPosition, SpawnRotation/*,laserSpawner.transform*/) as GameObject;
        LaserInstance.GetComponent<LaserScript>().speed = RB.velocity.magnitude + laserSpeed;
        LaserInstance.GetComponent<LaserScript>().damage = laserDamage;
    }
}
