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

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI leftTargetText;
    public TextMeshProUGUI rightTargetText;
    public PlayerController playerController;

    public float heightOffsetBelowHeadset = .2f; //how far below the headset to put the UI when changing the UI height
    public GameObject UIParent; //the empty parent we use to move all the UI up or down

    private float currentSpeed;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerPhysics = playerController.RB;
        LevelState.levelState.levelStatus = 1; //brute force the levelStatus to 1 (in play)
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
        if (LevelState.levelState.levelStatus == 1 && !GameplayUI.activeInHierarchy)
        {
            GameplayUI.SetActive(true);
        }
        else if (LevelState.levelState.levelStatus != 1 && GameplayUI.activeInHierarchy)
        {
            GameplayUI.SetActive(false);
        }
        
        if (LevelState.levelState.levelStatus == 0 && !MenuUI.activeInHierarchy)
        {
            MenuUI.SetActive(true);
        }
        else if (LevelState.levelState.levelStatus != 0 && MenuUI.activeInHierarchy)
        {
            MenuUI.SetActive(false);
        }

        Debug.Log("Level Status:" + LevelState.levelState.levelStatus);

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
}
