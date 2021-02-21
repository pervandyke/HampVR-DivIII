using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState : MonoBehaviour
{
    public int levelStatus = 0; //0 = start, 1 = in progress, 2 = win, 3 = lose


    void Update()
    {
        if (CheckEnemyStatus())
        {
            levelStatus = 2;
        }
    }

    private bool CheckEnemyStatus()
    {
        return EnemyManager.enemyManager.CheckEmpty();
    }
}
