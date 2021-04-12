using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class EngineModulator : MonoBehaviour
{
    public StudioEventEmitter engineEmitter;
    public VehicleMovement vehicleMovementScript;



    private void Update()
    {
        float speedPercentage = Mathf.InverseLerp(0, vehicleMovementScript.maxSpeed, vehicleMovementScript.RB.velocity.magnitude);
        engineEmitter.SetParameter("RPM", 2000 * speedPercentage);
    }
}
