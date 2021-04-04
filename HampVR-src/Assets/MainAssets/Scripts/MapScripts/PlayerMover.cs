using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.Find("[CameraRig]");
        player.transform.position = new Vector3(gameObject.transform.position.x, player.transform.position.y, gameObject.transform.position.z); ;
    }
}
