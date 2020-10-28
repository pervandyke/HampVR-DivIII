using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsLibrary : MonoBehaviour
{
    private void FireShotgun(GameObject spawner, Rigidbody sourceRB, Quaternion aimRotation, float laserSpeed, int laserDamage)
    {
        aimRotation.eulerAngles = new Vector3(0, aimRotation.eulerAngles.y, 0);
        Quaternion adjustedAimRotation = aimRotation;
        for (int i = -15; i < 14; i++)
        {
            adjustedAimRotation.y++;
            GenerateLaser("Prefabs/Laser", spawner, sourceRB, adjustedAimRotation, laserSpeed, laserDamage);
        }
    }

    private void GenerateLaser(string prefabPath, GameObject laserSpawner, Rigidbody sourceRB, Quaternion rotation, float laserSpeed, int laserDamage)
    {
        GameObject LaserInstance = Instantiate(Resources.Load<GameObject>(prefabPath), laserSpawner.transform.position, laserSpawner.transform.rotation) as GameObject;
        LaserInstance.transform.rotation = rotation;
        LaserInstance.GetComponent<LaserScript>().speed = sourceRB.velocity.magnitude + laserSpeed;
        LaserInstance.GetComponent<LaserScript>().damage = laserDamage;
    }
}
