using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCurveScript : MonoBehaviour
{

    public AnimationCurve accelerationCurve;

    void Start()
    {
    }

    void Update()
    {
        accelerationCurve.Evaluate(Time.time);
    }
}
