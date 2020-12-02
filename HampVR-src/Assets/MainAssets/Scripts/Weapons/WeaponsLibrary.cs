using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsLibrary : MonoBehaviour
{
    public static WeaponsLibrary wepLib;

    void Awake()
    {
        if (wepLib == null)
        {
            DontDestroyOnLoad(gameObject); //makes instance persist across scenes
            wepLib = this;
        }
        else if (wepLib != this)
        {
            Destroy(gameObject);
        }
    }
    public void FireShotgun(GameObject spawner, Rigidbody sourceRB, Quaternion aimRotation, float laserSpeed, int laserDamage)
    {
        aimRotation.eulerAngles = new Vector3(0, aimRotation.eulerAngles.y, 0);
        Quaternion adjustedAimRotation = aimRotation;
        for (int i = -10; i < 9; i++)
        {
            Vector3 aimVector = new Vector3(adjustedAimRotation.eulerAngles.x, adjustedAimRotation.eulerAngles.y + i, adjustedAimRotation.eulerAngles.z);
            Quaternion finalAim = new Quaternion();
            finalAim.eulerAngles = aimVector;
            GenerateLaser("Prefabs/Laser", spawner, sourceRB, finalAim, laserSpeed, laserDamage);
        }
    }

    private void GenerateLaser(string prefabPath, GameObject laserSpawner, Rigidbody sourceRB, Quaternion rotation, float laserSpeed, int laserDamage)
    {
        GameObject LaserInstance = Instantiate(Resources.Load<GameObject>(prefabPath), laserSpawner.transform.position, laserSpawner.transform.rotation) as GameObject;
        LaserInstance.transform.rotation = rotation;
        LaserInstance.GetComponent<LaserScript>().speed = sourceRB.velocity.magnitude + laserSpeed;
        LaserInstance.GetComponent<LaserScript>().damage = laserDamage;
    }

    public void FireLongRangeMissile(GameObject missileSpawner, Rigidbody sourceRB, Quaternion rotation, float missileSpeed, float missileTurningSpeed, int missileDamage)
    {
        
        GenerateMissile("Prefabs/Missile", missileSpawner, sourceRB, rotation, missileSpeed, missileTurningSpeed, missileDamage, Global.global.selectedTarget);
    }

    private void GenerateMissile(string prefabPath, GameObject missileSpawner, Rigidbody sourceRB, Quaternion rotation, float missileSpeed, float missileTurningSpeed, int missileDamage, GameObject missileTarget)
    {
        GameObject MissileInstance = Instantiate(Resources.Load<GameObject>(prefabPath), missileSpawner.transform.position, missileSpawner.transform.rotation) as GameObject;
        MissileScript missileScriptInstance = MissileInstance.GetComponent<MissileScript>();
        MissileInstance.transform.rotation = rotation;
        missileScriptInstance.speed = sourceRB.velocity.magnitude + missileSpeed;
        missileScriptInstance.turningSpeed = missileTurningSpeed;
        missileScriptInstance.damage = missileDamage;
        missileScriptInstance.target = missileTarget;
    }
}
