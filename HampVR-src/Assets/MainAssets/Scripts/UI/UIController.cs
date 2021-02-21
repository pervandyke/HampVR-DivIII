using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject player;
    private GameObject target;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI targetText;
    private PlayerController playerController;

    private float currentSpeed;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        gameObject.GetComponent<Canvas>().worldCamera = playerController.mainCamera;
        currentHealth = playerController.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Rigidbody>().velocity.magnitude != currentSpeed)
        {
            currentSpeed = player.GetComponent<Rigidbody>().velocity.magnitude;
            speedText.text = "Speed: " + (int)currentSpeed + "m/s";
        }
        if (playerController.health != currentHealth)
        {
            currentHealth = playerController.health;
            healthText.text = "Health: " + currentHealth;
        }
        if (Global.global.rightSelectedTarget != null)
        {
            targetText.text = "Target: " + Global.global.rightSelectedTarget.name;
        }
        else
        {
            targetText.text = "Target: ";
        }
    }
}
