//not in use
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_V1 : MonoBehaviour, IEnemy
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
        EnemyManager.enemyManager.AddEnemy(gameObject);
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
            EnemyManager.enemyManager.RemoveEnemy(gameObject);
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
        laserTimer -= Time.deltaTime;
        int bitMask = 1 << 9;
        bitMask = ~bitMask;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit forwardHit, 200.0f, bitMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 200.0f, Color.red);
            if (laserTimer <= 0)
            {
                Shoot();
                laserTimer = laserTimerDefault;
                print("Raycast hit");
                if (forwardHit.transform.gameObject.layer == 10 && forwardHit.transform.gameObject.tag == "Player")
                {
                    print("shooting");
                }
            }
        } else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 200.0f, Color.white);
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

    public void TakeDamage(int damage)
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
