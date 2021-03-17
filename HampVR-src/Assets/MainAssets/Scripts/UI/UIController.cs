using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    private Rigidbody playerPhysics;
    private GameObject target;
    
    public GameObject GameplayUI;
    public GameObject MenuUI;
    public GameObject EndUI;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI leftTargetText;
    public TextMeshProUGUI rightTargetText;
    public TextMeshProUGUI WinLoseMessage;
    public PlayerController playerController;

    private float currentSpeed;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerPhysics = playerController.RB;
    }

    // Update is called once per frame
    void Update()
    {
        ActiveUI();
        ShipStatusPanel();
        TargetPanel();
        StartPanel();
        LevelPanel();
    }

    private void ActiveUI()
    {
        if (LevelState.levelState.levelStatus == 0 && !MenuUI.activeInHierarchy)
        {
            MenuUI.SetActive(true);
        }
        else if (LevelState.levelState.levelStatus != 0 && MenuUI.activeInHierarchy)
        {
            MenuUI.SetActive(false);
        }

        if (LevelState.levelState.levelStatus == 1 && !GameplayUI.activeInHierarchy)
        {
            GameplayUI.SetActive(true);
        }
        else if (LevelState.levelState.levelStatus != 1 && GameplayUI.activeInHierarchy)
        {
            GameplayUI.SetActive(false);
        }

        if (LevelState.levelState.levelStatus == 2 && !EndUI.activeInHierarchy)
        {
            EndUI.SetActive(true);
        }
        else if (LevelState.levelState.levelStatus != 2 && EndUI.activeInHierarchy)
        {
            EndUI.SetActive(false);
        }

    }


    private void ShipStatusPanel()
    {
        if (LevelState.levelState.levelStatus == 1)
        {
            healthText.text = "HEALTH: " + playerController.health;

            speedText.text = "SPEED: " + (int)playerPhysics.velocity.magnitude;
        }
        
    }

    private void TargetPanel()
    {
        if (LevelState.levelState.levelStatus == 1)
        {
            if (!GameplayUI.activeInHierarchy)
            {
                GameplayUI.SetActive(true);
            }
            
        }
        else if (LevelState.levelState.levelStatus != 1 && GameplayUI.activeInHierarchy)
        {
            GameplayUI.SetActive(false);
        }
    }

    private void StartPanel()
    {
        if (LevelState.levelState.levelStatus == 0)
        {

        }
    }

    private void LevelPanel()
    {
        if (LevelState.levelState.levelStatus == 0)
        {

        }
    }

    public void UpdateEndStatePanel(string message)
    {
        WinLoseMessage.text = message;
    }

    public void ReturnToMenu()
    {
        LevelState.levelState.levelStatus = 0;
        LevelState.levelState.ProcessState();
    }
}