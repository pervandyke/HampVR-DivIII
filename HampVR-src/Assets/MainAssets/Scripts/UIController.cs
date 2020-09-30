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
    private PlayerMovementController playerController;

    private float currentSpeed;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerMovementController>();
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            target = EnemyManager.enemyManager.GetNextEnemy();
            if (target == null)
            {
                targetText.text = "Target: ";
            }
            else
            {
                targetText.text = "Target: " + target.name;
            } 
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //target closest enemy
        }
    }
}
