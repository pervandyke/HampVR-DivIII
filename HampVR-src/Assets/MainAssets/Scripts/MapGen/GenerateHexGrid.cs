using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class GenerateHexGrid : MonoBehaviour
{
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
        GameMap.gameMap.InitilizeMapData(mapData.mapWidth, mapData.mapHeight);

        //place hexes with defined positions
        foreach(HexPiece hex in mapData.tiles)
        {
            PlaceHex(hex.z, hex.x, hex.tileType);
            GameMap.gameMap.PlaceHex(hex);
        }

        List<Vector2> emptyHexes = GameMap.gameMap.GetEmptyHexes();
        List<string> presenceHexes = new List<string>();

        //ready hexes with defined presence but not location
        for (int i = 0; i < mapData.otherTileKeys.Count; i++)
        {
            for (int y = 0; y < mapData.otherTileValues[i]; y++)
            {
                presenceHexes.Add(mapData.otherTileKeys[i]);
            }
        }

        //shuffle presence hexes
        presenceHexes.Shuffle();
        //shuffle empty hexes
        emptyHexes.Shuffle();

        //place presence hexes
        foreach (string hex in presenceHexes)
        {
            if (emptyHexes.Count > 0)
            {
                Vector2 coords = emptyHexes[0];
                emptyHexes.RemoveAt(0);
                PlaceHex((int)coords.x, (int)coords.y, hex);
                GameMap.gameMap.PlaceHex(new HexPiece {z = (int)coords.x, x = (int)coords.y, tileType = hex});
            }
            else
            {
                break;
            }
        }

        //place filler hexes
        emptyHexes.Clear();
        //print("trying to call get empty hexes");
        emptyHexes = GameMap.gameMap.GetEmptyHexes();

        foreach (Vector2 hex in emptyHexes)
        {
            print("placing hex at: (" + hex.x + ", " + hex.y + ")");
            PlaceHex((int)hex.x, (int)hex.y, "HexFiller");
            GameMap.gameMap.PlaceHex(new HexPiece { z = (int)hex.x, x = (int)hex.y, tileType = "HexFiller" });
        }
    }

    //Modified from Cosmia https://github.com/HampshireCollegeCompSci/cs327_f2019
    public static MapObject CreateFromJSON(string path)
    {
        var jsonTextFile = Resources.Load<TextAsset>(path);
        return JsonUtility.FromJson<MapObject>(jsonTextFile.ToString());
    }

    private void PlaceHex(int hexZ, int hexX, string tileType)
    {
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = Vector3.zero;
        Vector3 position = new Vector3();
        position.x = hexX * hexHeight / 2;
        position.z = hexZ * hexWidth * .75f;
        position.y = 0;
        GameObject HexInstance = Instantiate(Resources.Load<GameObject>("Prefabs/"+tileType), position, rotation) as GameObject;
        HexInstance.transform.localScale = new Vector3(sideLength, 1.0f, sideLength);
        HexInstance.GetComponent<HexData>().x = hexX;
        HexInstance.GetComponent<HexData>().z = hexZ;
        if (HexInstance.TryGetComponent<HexInit>(out HexInit init))
        {
            init.SpawnTurrets(2);
        }
        HexInstance.transform.parent = GameObject.Find("MapHolder").transform;
    }
}
