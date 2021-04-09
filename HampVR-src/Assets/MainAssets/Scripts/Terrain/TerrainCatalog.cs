using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCatalog : MonoBehaviour
{
    public List<GameObject> terrainTypes;
    public GameObject terrain1;
    public GameObject terrain2;
    public GameObject terrain3;

    private void Start()
    {
        terrainTypes = new List<GameObject>();
        terrainTypes.Add(terrain1);
        terrainTypes.Add(terrain2);
        terrainTypes.Add(terrain3);
    }
}
