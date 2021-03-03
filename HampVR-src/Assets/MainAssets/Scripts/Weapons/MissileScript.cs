using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    public float speed;
    public float turningSpeed;
    public int damage;
    public string missileType;
    public GameObject target;
    private float timer = 15;

    private void FixedUpdate()
    {
        if (target != null)
        {
            //rotate to face target
            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * turningSpeed);
        }

        //apply forward force
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speed;

        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //playParticle();

        if (collision.gameObject.tag == "Enemy")
        {
            print("Hit enemy");
            collision.gameObject.GetComponent<IEnemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            print("Hit something");
            Destroy(gameObject);
        }


        //collisionflag = true;
    }
}
