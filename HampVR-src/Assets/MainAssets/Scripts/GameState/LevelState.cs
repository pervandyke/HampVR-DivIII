using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState : MonoBehaviour
{
    public static LevelState levelState;
    public int levelStatus; //0 = menu, 1 = in progress, 2 = level complete (win/lose)
    public PlayerController PC;
    public UIController UIC;

    private void Awake()
    {
        levelState = this;
    }
    private void Start()
    {
        PC = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        UIC = GameObject.Find("UIController").GetComponent<UIController>();
    }
    void Update()
    {
        if (CheckEnemyEmpty() && levelStatus == 1)
        {
            levelStatus = 2;
            ProcessState("Win");
        }
        if (PC.health <= 0 && levelStatus == 1)
        {
            levelStatus = 2;
            ProcessState("Lose");
        }
        
    }

    private bool CheckEnemyEmpty()
    {
        return EnemyManager.enemyManager.CheckEmpty();
    }

    public void ProcessState(string endGameStatus = "Default")
    {
        if (levelStatus == 0)
        {
            //lock rotation of console, if canopy isn't grey, grey it

        }
        else if (levelStatus == 1)
        {
            //un-grey canopy, and unlock rotation
            Global.global.rotationType = "absolute";
        }
        else if (levelStatus == 2)
        {
            //grey out canopy and display you win/you lose message on consoles
            //after timer switch to state 0

            //grey out canopy
            UIC.UpdateEndStatePanel("You " + endGameStatus);
            Global.global.rotationType = "none";
            GameObject.Find("PlayerModel").transform.rotation = GameObject.Find("Camera").transform.rotation;
        }
    }
}
