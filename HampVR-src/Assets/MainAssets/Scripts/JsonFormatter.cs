using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonFormatter : MonoBehaviour
{
    MapObject testMap;

    void Start()
    {
        testMap = new MapObject()
        {
            tiles = new List<HexPiece>()
        };

        for (int i = 0; i < 20; i++)
        {
            HexPiece hex = new HexPiece();
            hex.tileType = "default";
            hex.x = 0;
            hex.z = 0;
            testMap.tiles.Add(hex);
        }

        string json = JsonUtility.ToJson(testMap, true);
        if (Application.isEditor)
        {
            File.WriteAllText("Assets/MainAssets/Resources/JSONS/TestMapData.json", json);
        }
    }
}
