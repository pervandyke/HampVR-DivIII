using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDestroyer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Destroy(gameObject);
        }
    }
}
