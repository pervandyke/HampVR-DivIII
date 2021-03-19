using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Global : MonoBehaviour
{
    public static Global global;

    public string rotationType;
    public List<List<HexPrefabReference>> hexSelectionLists;
    public GameObject leftSelectedTarget;
    public GameObject rightSelectedTarget;
    public SteamVR_Action_Vibration hapticAction;

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

    public void HapticPulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        //hapticAction.Execute(0, duration, frequency, amplitude, source);
    }

}

//Extension taken from this post on stack overflow: https://stackoverflow.com/questions/273313/randomize-a-listt
public static class Extensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
