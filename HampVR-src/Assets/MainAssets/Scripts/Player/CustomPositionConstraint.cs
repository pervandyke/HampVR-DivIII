using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPositionConstraint : MonoBehaviour
{

    public Transform model;
    public Transform cockpit;
    public float height;


    void LateUpdate()
    {
        cockpit.position = new Vector3(model.position.x, height, model.position.z);
    }
}
