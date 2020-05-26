using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{

    public float speed = 10;
    private Rigidbody RB;
    private float timer = 15;

    void Start()
    {
        RB = gameObject.GetComponent<Rigidbody>();
        transform.parent = null;
    }
    void FixedUpdate()
    {
        RB.velocity = transform.up * speed;
        timer = timer - Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Bang!");
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
