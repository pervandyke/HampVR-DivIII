using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{

    public float speed = 30;
    private Rigidbody RB;
    private float timer = 15;
    private float collisionTimer = 0.5f;
    private bool collisionflag = false;

    void Start()
    {
        RB = gameObject.GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        RB.velocity = transform.up * speed;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
        if (collisionflag)
        {
            collisionTimer -= Time.deltaTime;
            if (collisionTimer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void playParticle()
    {
        print("Bang!");
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        RB.velocity = Vector3.zero;
        GetComponentInChildren<ParticleSystem>().Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        playParticle();
        collisionflag = true;
    }
}
