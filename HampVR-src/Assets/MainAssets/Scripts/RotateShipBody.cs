using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateShipBody : MonoBehaviour
{

    public Transform cameraTransform;
    void Update()
    {
        transform.rotation = cameraTransform.rotation;
    }
}
