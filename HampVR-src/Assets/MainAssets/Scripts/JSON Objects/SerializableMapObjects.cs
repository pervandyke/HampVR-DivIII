using System.IO;

[System.Serializable]
public class MapObject
{
    public HexPiece[] tiles;
}
[System.Serializable]
public class HexPiece
{
    int[] coordinates;
    string catagory;
}
