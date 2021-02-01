using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    private Hashtable map;
    private Dictionary<Vector2, HexPiece> dictMap;
    private int width;
    private int height;
    public static GameMap gameMap;

    private void Awake()
    {
        gameMap = this;
        
    }

    public void InitilizeMapData(int mapWidth, int mapHeight)
    {
        dictMap = new Dictionary<Vector2, HexPiece>();
        width = mapWidth;
        height = mapHeight;
    }

    public void PlaceHex(HexPiece hex)
    {
        if (dictMap.ContainsKey(new Vector2(hex.z, hex.x)))
        {
            dictMap[new Vector2(hex.z, hex.x)] = hex;
        }
        else
        {
            dictMap.Add(new Vector2(hex.z, hex.x), hex);
        }
        
    }

    public HexPiece GetHexData(int column, int row)
    {
        if (!dictMap.ContainsKey(new Vector2(column, row)))
        {
            return null;
        }
        else
        {
            return dictMap[new Vector2(column, row)];
        }
    }

    public List<Vector2> GetEmptyHexes()
    {
        List<Vector2> emptyHexes = new List<Vector2>();
        for (int i = 0; i < width; i++)
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
            for (int y = start; y < height; y = y + 2)
            {
                if (GetHexData(i, y) == null)
                {
                    emptyHexes.Add(new Vector2(i, y));
                }
            }
        }
        return emptyHexes;
    }
}
