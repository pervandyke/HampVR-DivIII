using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
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
        RB.velocity = transform.forward * speed;
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

    private void OnTriggerEnter(Collider other)
    {
        playParticle();
        if (other.gameObject.layer == 10)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponentInChildren<PlayerMovementController>().health -= damage;
            }
            if (other.gameObject.tag == "Enemy")
            {
                print("Hit enemy");
                other.gameObject.GetComponent<IEnemy>().TakeDamage(damage);
            }
        }
        Destroy(gameObject);
        //collisionflag = true;
    }
}
