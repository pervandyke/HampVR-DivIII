using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseLerpTest : MonoBehaviour
{
    private void Start()
    {
        ILerpTest();
    }
    void ILerpTest()
    {
        float output = 0;
        output = Mathf.InverseLerp(0, 100, -10);
        print(output);
    }
}
