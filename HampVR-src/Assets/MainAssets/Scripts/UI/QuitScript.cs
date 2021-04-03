using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitScript : MonoBehaviour
{
    public float timerDefault;
    public bool buttonHeld;
    private float buttonTimer;
    public UIController UIC;

    private void Start()
    {
        buttonTimer = timerDefault;
    }
    private void OnTriggerEnter(Collider other)
    {
        buttonHeld = true;
        gameObject.GetComponent<Image>().color = new Color(0.7843137f, 0.7843137f, 0.7843137f);
    }

    private void OnTriggerExit(Collider other)
    {
        buttonHeld = false;
        gameObject.GetComponent<Image>().color = Color.white;
    }

    private void Update()
    {
        if (buttonHeld)
        {
            buttonTimer -= Time.deltaTime;
            if (buttonTimer <= 0)
            {
                buttonTimer = timerDefault;
                buttonHeld = false;
                print("Quitting");
                Application.Quit();
            }
        }
        else if (!buttonHeld && buttonTimer != timerDefault)
        {
            buttonTimer = timerDefault;
        }
    }
}