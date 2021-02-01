using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class GenerateHexGrid : MonoBehaviour
{
    [Tooltip("The number of rings of hexagons around the center.")]
    public int layers;
    [Tooltip("The side length of the hexes being used.")]
    public int sideLength;

    public List<GameObject> hexTypes;

    private float hexHeight;
    private float hexWidth;

    private MapObject mapData;
    
    private void Start()
    {
        hexHeight = Mathf.Sqrt(3.0f) * sideLength;
        hexWidth = sideLength * 2;
        mapData = CreateFromJSON("JSONS/TestMapData");
        GenerateMap(mapData);
    }

    private void GenerateMap(MapObject mapData)
    {
        print("Width: " + mapData.mapWidth);
        print("Height: " + mapData.mapHeight);
        GameMap.gameMap.InitilizeMapData(mapData.mapWidth, mapData.mapHeight);

        foreach(HexPiece hex in mapData.tiles)
        {
            PlaceHex(hex.x, hex.z, hex.tileType);
            GameMap.gameMap.PlaceHex(hex.x, hex.z, hex);
        }

        for (int i = 0; i < mapData.mapHeight; i++)
        {
            int start;
            if (i % 2 == 0)
            {
                start = 0;
            }
            else
            {
                start = 1;
            }
            for (int y = start; y < mapData.mapWidth; y = y + 2)
            {
                if (GameMap.gameMap.GetHexData(i,y) == null)
                {
                    PlaceHex(i, y, "HexFiller");
                    GameMap.gameMap.PlaceHex(i, y, new HexPiece {z = i, x = y, tileType = "HexFiller"});
                }
            }
        }
    }

    //Modified from Cosmia https://github.com/HampshireCollegeCompSci/cs327_f2019
    public static MapObject CreateFromJSON(string path)
    {
        var jsonTextFile = Resources.Load<TextAsset>(path);
        return JsonUtility.FromJson<MapObject>(jsonTextFile.ToString());
    }

    private void PlaceHex(int hexX, int hexZ, string tileType)
    {
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = Vector3.zero;    
        Vector3 position = new Vector3();
        position.x = hexX * hexHeight / 2;
        position.z = hexZ * hexWidth * .75f;
        position.y = 0;
        GameObject HexInstance = Instantiate(Resources.Load<GameObject>("Prefabs/"+tileType), position, rotation) as GameObject;
        HexInstance.GetComponent<HexData>().x = hexX;
        HexInstance.GetComponent<HexData>().z = hexZ;
    }
}
