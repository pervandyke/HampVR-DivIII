using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateShipBody : MonoBehaviour
{

    public Transform cameraTransform;
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, cameraTransform.rotation.eulerAngles.y, 0);
    }
}
