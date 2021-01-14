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
}
[System.Serializable]
public class HexPiece
{
    public int x;
    public int z;
    public string tileType;
}
