using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour, IEnemy
{
    public GameObject barrel;
    public GameObject pivot;
    public GameObject cap;
    public GameObject body;

    private GameObject player;
    private Quaternion lookRotation;
    private Vector3 direction;

    public int health;
    public float trackingSpeed;
    public float trackingDistance;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TrackPlayer()
    {
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) < trackingDistance)
        {
            WhereToRotate(player.transform.position);

            cap.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * trackingSpeed);
            cap.transform.rotation.eulerAngles.Set(0, cap.transform.rotation.eulerAngles.y, 0);

            pivot.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * trackingSpeed);
            pivot.transform.rotation.eulerAngles.Set(pivot.transform.rotation.eulerAngles.x, 0, 0);
        }
    }

    private void WhereToRotate(Vector3 target)
    {
        direction = (target - transform.position).normalized;
        lookRotation = Quaternion.LookRotation(direction);
    }



    public void TakeDamage(int damage)
    {
        health -= damage;
        print("applied damage");
    }
}
