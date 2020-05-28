﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryMovementController : MonoBehaviour
{

    public GameObject player;
    public GameObject laserSpawner;
    public GameObject laserSpawner2;
    private Rigidbody RB;
    private Quaternion originalRotation;

    public float acceleration;
    public float decceleration;
    public float strafeSpeed;
    public float rotateSpeed;

    public float laserSpeed;
    public int laserDamage;

    public int health;


    // Start is called before the first frame update
    void Start()
    {
        RB = player.GetComponent<Rigidbody>();
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Acceleration
        if (Input.GetKey(KeyCode.LeftShift))
        {
            RB.AddRelativeForce(Vector3.up * acceleration);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            RB.AddRelativeForce(Vector3.up * decceleration);
        }

        //Strafe
        if (Input.GetKey(KeyCode.Space))
        {
            RB.AddRelativeForce(Vector3.forward * -strafeSpeed);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            RB.AddRelativeForce(Vector3.forward * strafeSpeed);
        }

        //Strafe
        if (Input.GetKey(KeyCode.Z))
        {
            RB.AddRelativeForce(Vector3.right * -strafeSpeed);
        }
        else if (Input.GetKey(KeyCode.X))
        {
            RB.AddRelativeForce(Vector3.right * strafeSpeed);
        }

        //Pitch
        if (Input.GetKey(KeyCode.W))
        {
            player.transform.Rotate(new Vector3(rotateSpeed, 0, 0));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            player.transform.Rotate(new Vector3(-rotateSpeed, 0, 0));
        }

        //Rotation
        if (Input.GetKey(KeyCode.A))
        {
            player.transform.Rotate(new Vector3(0,0,rotateSpeed));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            player.transform.Rotate(new Vector3(0,0,-rotateSpeed));
        }

        //Roll
        if (Input.GetKey(KeyCode.Q))
        {
            player.transform.Rotate(new Vector3(0, rotateSpeed, 0));
        }
        else if (Input.GetKey(KeyCode.E))
        {
            player.transform.Rotate(new Vector3(0, -rotateSpeed, 0));
        }
        /*else
        {
            if (player.transform.localRotation.eulerAngles.x > 91)
            {
                player.transform.Rotate(new Vector3(0, rotateSpeed, 0));
            }
            else if (player.transform.localRotation.eulerAngles.x < 89)
            {
                player.transform.Rotate(new Vector3(0, -rotateSpeed, 0));
            }
        }
        */
        
    }

    private void Update()
    {
        //Shooting
        if (Input.GetKeyDown(KeyCode.F))
        {
            Shoot();
        }

        if (health <= 0)
        {
            Destroy(player);
        }
    }

    public void Shoot()
    {
        Vector3 SpawnPosition = laserSpawner.transform.position;
        Quaternion SpawnRotation = laserSpawner.transform.rotation;
        GameObject LaserInstance;
        LaserInstance = Instantiate(Resources.Load<GameObject>("Prefabs/Laser"), SpawnPosition, SpawnRotation) as GameObject;
        LaserInstance.GetComponent<LaserScript>().speed = RB.velocity.magnitude + laserSpeed;
        LaserInstance.GetComponent<LaserScript>().damage = laserDamage;
        SpawnPosition = laserSpawner2.transform.position;
        SpawnRotation = laserSpawner2.transform.rotation;
        LaserInstance = Instantiate(Resources.Load<GameObject>("Prefabs/Laser"), SpawnPosition, SpawnRotation) as GameObject;
        LaserInstance.GetComponent<LaserScript>().speed = RB.velocity.magnitude + laserSpeed;
        LaserInstance.GetComponent<LaserScript>().damage = laserDamage;
    }
}
