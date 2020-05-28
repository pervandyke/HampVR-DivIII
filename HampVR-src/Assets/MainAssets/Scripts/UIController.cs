using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public GameObject healthDisplay;
    public GameObject speedDisplay;
    public GameObject player;

    private Text healthText;
    private Text speedText;

    private float currentSpeed;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        healthText = healthDisplay.GetComponent<Text>();
        speedText = speedDisplay.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Rigidbody>().velocity.magnitude != currentSpeed)
        {
            currentSpeed = player.GetComponent<Rigidbody>().velocity.magnitude;
            speedText.text = "Speed: " + currentSpeed + "m/s";
        }
        if (GameObject.Find("TempPlayerController").GetComponent<TemporaryMovementController>().health != currentHealth)
        {
            currentHealth = GameObject.Find("TempPlayerController").GetComponent<TemporaryMovementController>().health;
            healthText.text = "Health: " + currentHealth;
        }
    }
}
