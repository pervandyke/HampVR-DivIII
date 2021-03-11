using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState : MonoBehaviour
{
    public static LevelState levelState;
    public int levelStatus = 1; //0 = menu, 1 = in progress, 2 = level complete (win/lose)

    private void Awake()
    {
        levelState = this;
    }
    void Update()
    {
        if (CheckEnemyStatus())
        {
            levelStatus = 2;
        }
        ProcessState();
    }

    private bool CheckEnemyStatus()
    {
        return EnemyManager.enemyManager.CheckEmpty();
    }

    private void ProcessState()
    {
        if (levelStatus == 2)
        {
            // grey out canopy and display you win message on consoles
        }
        else if (levelStatus == 3)
        {
            // grey out canopy and display you lose message on consoles
        }
    }
}
