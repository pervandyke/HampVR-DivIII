using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    void Start()
    {
        List<GameObject> availableTerrain = GameObject.Find("TerrainCatalog").GetComponent<TerrainCatalog>().terrainTypes;
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        Instantiate(availableTerrain[Random.Range(0, availableTerrain.Count)], gameObject.transform.position, rotation);
    }
}
