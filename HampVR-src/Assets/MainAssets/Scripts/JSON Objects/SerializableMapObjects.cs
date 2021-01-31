using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MapObject
{
    public List<HexPiece> tiles;
    public List<string> otherTileKeys;
    public List<int> otherTileValues;
    public int mapWidth;
    public int mapHeight;
}
[System.Serializable]
public class HexPiece
{
    public int z;
    public int x;
    public string tileType;
}
