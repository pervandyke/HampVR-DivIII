using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnToMenuScript : MonoBehaviour
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
                UIC.ReturnToMenu();
            }
        }
        else if (!buttonHeld && buttonTimer != timerDefault)
        {
            buttonTimer = timerDefault;
        }
    }
}
