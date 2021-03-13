using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class EngineModulator : MonoBehaviour
{
    public StudioEventEmitter engineEmitter;
    public PlayerController PC;



    private void Update()
    {
        float speedPercentage = Mathf.InverseLerp(0, PC.maxSpeed, PC.RB.velocity.magnitude);
        engineEmitter.SetParameter("RPM", 2000 * speedPercentage);
    }
}
