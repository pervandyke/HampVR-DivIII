using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = Instantiate(Resources.Load("Prefabs/[CameraRig]") as GameObject, transform.position, transform.rotation);
    }
}
