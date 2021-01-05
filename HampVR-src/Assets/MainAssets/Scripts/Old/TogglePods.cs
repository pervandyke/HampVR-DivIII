using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePods : MonoBehaviour
{

    public Animator podAnimator;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            print("Pressed L");
            print("Extended: " + podAnimator.GetBool("Extended"));
            print("Spinning: " + podAnimator.GetBool("Spinning"));
            if (podAnimator.GetBool("Extended") == false)
            {
                podAnimator.SetBool("Extended", true);
            }
            else
            {
                podAnimator.SetBool("Extended", false);
            }
            
            if (podAnimator.GetBool("Spinning") == false)
            {
                podAnimator.SetBool("Spinning", true);
            }
            else
            {
                podAnimator.SetBool("Spinning", false);
            }
            print("Extended: " + podAnimator.GetBool("Extended"));
            print("Spinning: " + podAnimator.GetBool("Spinning"));

        }
    }
}
