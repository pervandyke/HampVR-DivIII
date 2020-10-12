using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MapObject
{
    public List<HexPiece> tiles;
}
[System.Serializable]
public class HexPiece
{
    public int x;
    public int z;
    public string tileType;
}
