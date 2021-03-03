using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawnerScript : MonoBehaviour
{
    public void SpawnTurret()
    {
        GameObject turretInstance = Instantiate(Resources.Load("Prefabs/Turret")) as GameObject;
        turretInstance.transform.position = gameObject.transform.position;
        turretInstance.transform.parent = gameObject.transform;
    }
}