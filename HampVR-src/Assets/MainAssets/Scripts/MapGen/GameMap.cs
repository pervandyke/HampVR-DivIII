using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    private Hashtable map;
    public static GameMap gameMap;

    private void Awake()
    {
        gameMap = this;
    }

    public void InitilizeMapData(int columns, int rows)
    {
        map = new Hashtable();
    }

    public void PlaceHex(int column, int row, HexPiece hex)
    {
        map.Add(new Vector2(column, row), hex);
    }

    public HexPiece GetHexData(int column, int row)
    {
        return (HexPiece)map[new Vector2(column, row)];
    }
}
