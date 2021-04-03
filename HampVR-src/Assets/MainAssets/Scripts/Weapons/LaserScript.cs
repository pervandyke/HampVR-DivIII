using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour, IProjectile
{

    public float speed = 30;
    public int damage;
    private Rigidbody RB;
    private float timer = 15;
    /*private float collisionTimer = 0.0f;
    private bool collisionflag = false;*/

    void Start()
    {
        RB = gameObject.GetComponent<Rigidbody>();
        
    }
    void FixedUpdate()
    {
        MovementControl(speed, 0.0f);
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
        /*
        if (collisionflag)
        {
            collisionTimer -= Time.deltaTime;
            if (collisionTimer <= 0)
            {
                Destroy(gameObject);
            }
        }*/
    }

    private void playParticle()
    {
        print("Bang!");
        RB.velocity = Vector3.zero;
        //GetComponentInChildren<ParticleSystem>().Play();
    }

    public void MovementControl(float speed, float turning)
    {
        RB.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        playParticle();
        if (other.gameObject.layer == 10)
        {
            print("Hit player");
            other.gameObject.GetComponentInChildren<PlayerController>().TakeHit(damage, gameObject);
            Destroy(gameObject);
        }
        
        //collisionflag = true;
    }
}