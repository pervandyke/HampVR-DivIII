using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSphereScript : MonoBehaviour
{
    public float timer = 10.0f;
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
