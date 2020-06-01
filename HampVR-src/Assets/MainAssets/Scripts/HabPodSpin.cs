using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabPodSpin : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.right, 0.1f, Space.Self);
    }
}
