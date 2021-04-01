using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCleanupScript : MonoBehaviour
{
    public void ClearMap()
    {
        Component[] mapObjects = gameObject.GetComponentsInChildren<HexData>();
        foreach (Component c in mapObjects)
        {
            Destroy(c.gameObject);
        }
    }
}
