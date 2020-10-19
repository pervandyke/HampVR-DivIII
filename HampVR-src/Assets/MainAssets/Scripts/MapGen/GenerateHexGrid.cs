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

    private Vector3 currentPosition;
    private float hexHeight;
    private float hexWidth;

    private MapObject mapData;
    
    private void Start()
    {
        currentPosition = Vector3.zero;
        hexHeight = Mathf.Sqrt(3.0f) * sideLength;
        hexWidth = sideLength * 2;
        mapData = CreateFromJSON("JSONS/TestMapData");
        GenerateMap(mapData);
        //CreateGrid();
        //Add back in once maps are set up
        //GenerateMap(CreateFromJSON("JSONS/TestMapData"));
    }

    private void CreateGrid()
    {
        PlaceHexOld();
        if (layers > 0)
        {
            for (int i = 0; i < layers; i++)
            {
                int hexesPlaced = 0;
                currentPosition.x = currentPosition.x - hexHeight;
                PlaceHexOld();
                hexesPlaced++;
                for (int j = 0; j <= i; j++)
                {
                    currentPosition.x = currentPosition.x + (hexHeight/2);
                    currentPosition.z = currentPosition.z + (hexWidth * .75f);
                    PlaceHexOld();
                    hexesPlaced++;
                }
                for (int j = 0; j <= i; j++)
                {
                    currentPosition.x = currentPosition.x + (hexHeight);
                    PlaceHexOld();
                    hexesPlaced++;
                }
                for (int j = 0; j <= i; j++)
                {
                    currentPosition.x = currentPosition.x + (hexHeight / 2);
                    currentPosition.z = currentPosition.z - (hexWidth * .75f);
                    PlaceHexOld();
                    hexesPlaced++;
                }
                for (int j = 0; j <= i; j++)
                {
                    currentPosition.x = currentPosition.x - (hexHeight / 2);
                    currentPosition.z = currentPosition.z - (hexWidth * .75f);
                    PlaceHexOld();
                    hexesPlaced++;
                }
                for (int j = 0; j <= i; j++)
                {
                    currentPosition.x = currentPosition.x - (hexHeight);
                    PlaceHexOld();
                    hexesPlaced++;
                }
                for (int j = 0; j <= i; j++)
                {
                    currentPosition.x = currentPosition.x - (hexHeight / 2);
                    currentPosition.z = currentPosition.z + (hexWidth * .75f);
                    if (hexesPlaced == 6 * (i + 1))
                    {
                        break;
                    }
                    PlaceHexOld();
                    hexesPlaced++;
                }
            }
        }
    }

    private void GenerateMap(MapObject mapData)
    {
        foreach(HexPiece hex in mapData.tiles)
        {
            PlaceHex(hex.x, hex.z, hex.tileType);
        }
    }

    //Modified from Cosmia https://github.com/HampshireCollegeCompSci/cs327_f2019
    public static MapObject CreateFromJSON(string path)
    {
        var jsonTextFile = Resources.Load<TextAsset>(path);
        return JsonUtility.FromJson<MapObject>(jsonTextFile.ToString());
    }

    private void PlaceHexOld()
    {
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = Vector3.zero;
        GameObject HexInstance = Instantiate(Resources.Load<GameObject>("Prefabs/Hex"), currentPosition, rotation) as GameObject;
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
