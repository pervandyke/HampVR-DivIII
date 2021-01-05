using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    public GameObject player;
    public float forwardOffset;

    private void LateUpdate()
    {
        camera.transform.position = player.transform.position + new Vector3(0,0,forwardOffset);
        camera.transform.rotation = player.transform.rotation;
    }
}
