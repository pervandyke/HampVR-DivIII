using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static Global global;

    public string rotationType = "relative";
    public List<List<HexPrefabReference>> hexSelectionLists;

    void Awake()
    {
        if (global == null)
        {
            DontDestroyOnLoad(gameObject); //makes instance persist across scenes
            global = this;
        }
        else if (global != this)
        {
            Destroy(gameObject); //deletes copies of global which do not need to exist, so right version is used to get info from
        }
    }


}
