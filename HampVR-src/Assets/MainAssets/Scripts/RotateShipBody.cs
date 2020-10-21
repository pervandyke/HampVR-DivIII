using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateShipBody : MonoBehaviour
{

    public Transform cameraTransform;
    void Update()
    {
        transform.rotation.eulerAngles.Set(transform.rotation.eulerAngles.x, cameraTransform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
