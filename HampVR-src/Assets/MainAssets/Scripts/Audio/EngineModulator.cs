using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class EngineModulator : MonoBehaviour
{
    public StudioEventEmitter engineEmitter;
    public VehicleMovement PC;



    private void Update()
    {
        float speedPercentage = Mathf.InverseLerp(0, PC.maxSpeed, PC.RB.velocity.magnitude);
        engineEmitter.SetParameter("RPM", 2000 * speedPercentage);
    }
}
