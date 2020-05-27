using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_V1 : MonoBehaviour
{
    public GameObject laserSpawner;
    private Rigidbody RB;
    private GameObject player;

    public float acceleration;
    public float decceleration;
    public float strafeSpeed;
    public float rotateSpeed;

    public float laserSpeed;
    public float laserTimerDefault;
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
        transform.LookAt(player.transform);
        if (DistanceToTarget(player) > 15)
        {
            RB.AddRelativeForce(Vector3.forward * acceleration);
        }
        else
        {
            RB.AddRelativeForce(Vector3.forward * decceleration);
        }

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
    }
}
