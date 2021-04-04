using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexInit : MonoBehaviour
{
    public void SpawnTurrets(int turretCount)
    {
        TurretSpawnerScript[] turretSpawners = gameObject.GetComponentsInChildren<TurretSpawnerScript>();
        List<TurretSpawnerScript> spawnerList = new List<TurretSpawnerScript>();
        foreach (TurretSpawnerScript s in turretSpawners)
        {
            spawnerList.Add(s);
        }
        
        
        for (int i = 0; i < turretCount; i++)
        {
            if (spawnerList.Count <= 0)
            {
                break;
            }
            print("spawner list count = " + spawnerList.Count);
            int turretIndex = Random.Range(0, spawnerList.Count);
            print("turret index = " + turretIndex);
            spawnerList[turretIndex].SpawnTurret();
            spawnerList.RemoveAt(turretIndex);
        }
    }
}
