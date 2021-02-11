using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
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
        print("getting empty hexes");
        List<Vector2> emptyHexes = new List<Vector2>();
        int workingWidth = width;
        int workingHeight = height;
        bool oddWidth = false;
        bool oddHeight = false;
        int oddWidthAdjustment = 0;
        int oddHeightAdjustment = 0;

        if (width % 2 != 0)
        {
            print("width is odd");
            workingWidth = width + 1;
            oddWidth = true;
            oddWidthAdjustment = 1;
        }
        if (height % 2 != 0)
        {
            print("height is odd");
            workingHeight = height + 1;
            oddHeight = true;
            oddHeightAdjustment = 1;
        }
        for (int i = -(workingWidth / 2); i < (workingWidth / 2) - oddWidthAdjustment; i++)
        {
            if (oddWidth && i + 1 == workingWidth)
            {
                break;
            }

            int start;
            if (i % 2 == 0)
            {
                start = -(workingHeight / 2);
            }
            else
            {
                start = -(workingHeight / 2) + 1;
            }

            for (int y = start; y < (workingHeight / 2) - oddHeightAdjustment; y = y + 2)
            {
                if (oddHeight && y + 1 == workingHeight)
                {
                    break;
                }
                if (GetHexData(i, y) == null)
                {
                    emptyHexes.Add(new Vector2(i, y));
                }
            }   
        }
        return emptyHexes;
    }
}
