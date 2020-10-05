using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateHexGrid : MonoBehaviour
{
    [Tooltip("The number of rings of hexagons around the center.")]
    public int layers;
    [Tooltip("The side length of the hexes being used.")]
    public int sideLength;

    private Vector3 currentPosition;
    private int currentLayer;
    private bool generating;
    private float hexHeight;
    private float hexWidth;

    private void Start()
    {
        currentPosition = Vector3.zero;
        hexHeight = Mathf.Sqrt(3.0f) * sideLength;
        hexWidth = sideLength * 2;
        CreateGrid();
    }

    private void CreateGrid()
    {
        PlaceHex();
        if (layers > 0)
        {
            for (int i = 0; i < layers; i++)
            {
                int hexesPlaced = 0;
                currentPosition.x = currentPosition.x - hexHeight;
                PlaceHex();
                hexesPlaced++;
                for (int j = 0; j <= i; j++)
                {
                    currentPosition.x = currentPosition.x + (hexHeight/2);
                    currentPosition.z = currentPosition.z + (hexWidth * .75f);
                    PlaceHex();
                    hexesPlaced++;
                }
                for (int j = 0; j <= i; j++)
                {
                    currentPosition.x = currentPosition.x + (hexHeight);
                    PlaceHex();
                    hexesPlaced++;
                }
                for (int j = 0; j <= i; j++)
                {
                    currentPosition.x = currentPosition.x + (hexHeight / 2);
                    currentPosition.z = currentPosition.z - (hexWidth * .75f);
                    PlaceHex();
                    hexesPlaced++;
                }
                for (int j = 0; j <= i; j++)
                {
                    currentPosition.x = currentPosition.x - (hexHeight / 2);
                    currentPosition.z = currentPosition.z - (hexWidth * .75f);
                    PlaceHex();
                    hexesPlaced++;
                }
                for (int j = 0; j <= i; j++)
                {
                    currentPosition.x = currentPosition.x - (hexHeight);
                    PlaceHex();
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
                    PlaceHex();
                    hexesPlaced++;
                }
            }
        }
    }

    private void PlaceHex()
    {
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = Vector3.zero;
        GameObject HexInstance = Instantiate(Resources.Load<GameObject>("Prefabs/Hex"), currentPosition, rotation) as GameObject;
    }
}
