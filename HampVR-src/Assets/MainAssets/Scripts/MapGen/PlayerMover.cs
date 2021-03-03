using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.Find("[CameraRig]");
        player.transform.position = gameObject.transform.position;
    }
}
