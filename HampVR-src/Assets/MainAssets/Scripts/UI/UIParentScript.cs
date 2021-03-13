using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParentScript : MonoBehaviour
{
    public float heightOffsetBelowHeadset = .2f; //how far below the headset to put the UI when changing the UI height

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeUIHeight(Vector3 headsetZero)
    {
        transform.localPosition = new Vector3(0, headsetZero.y - heightOffsetBelowHeadset, 0);
    }
}
