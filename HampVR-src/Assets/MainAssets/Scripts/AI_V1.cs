using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_V1 : MonoBehaviour
{
    public GameObject laserSpawner;
    private Rigidbody RB;
    private GameObject player;
    private Vector3 direction;
    private Quaternion lookRotation;

    public float speed;
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
        player = GameObject.Find("Player");
        RB = gameObject.GetComponent<Rigidbody>();
        laserTimer = laserTimerDefault;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        WhereToRotate(player);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        DecisionTree();
        
    }

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void WhereToRotate(GameObject target)
    {
        direction = (target.transform.position - transform.position).normalized;
        lookRotation = Quaternion.LookRotation(direction);
    }
    private void DecisionTree()
    {
        if (DistanceToTarget(player) > 5 && RB.velocity.magnitude != speed)
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

        if (DistanceToTarget(player) < 40)
        {
            laserTimer -= Time.deltaTime;
            if (laserTimer <= 0)
            {
                Shoot();
                laserTimer = laserTimerDefault;
            }

        }
    }

    public void DoDamage(int damage)
    {
        health -= damage;
        print("applied damage");
    }

    private float DistanceToTarget(GameObject target)
    {
        Vector3 tp = target.transform.position;
        Vector3 p = transform.position;
        float distance = Mathf.Sqrt(((p.x - tp.x) * (p.x - tp.x)) + ((p.y - tp.y) * (p.y - tp.y)) + ((p.z - tp.z) * (p.z - tp.z)));

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
