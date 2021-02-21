using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemies;
    private int selectedEnemyIndex;
    private GameObject selectedEnemy;

    public static EnemyManager enemyManager;

    private void Awake()
    {
        if (enemyManager == null)
        {
            //DontDestroyOnLoad(gameObject); //makes instance persist across scenes
            enemyManager = this;
        }
        else if (enemyManager != this)
        {
            Destroy(gameObject); //deletes copies of global which do not need to exist, so right version is used to get info from
        }
        enemies = new List<GameObject>();
    }
    private void Start()
    {
        
        selectedEnemyIndex = -1;
    }
    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    public GameObject GetNextEnemy()
    {
        if (enemies.Count == 0)
        {
            return null;
        }
        else if (selectedEnemyIndex == -1)
        {
            selectedEnemyIndex = 0;
            
        }
        else if (selectedEnemyIndex + 1 < enemies.Count)
        {
            selectedEnemyIndex += 1;
        }
        else
        {
            selectedEnemyIndex = 0;
        }
        selectedEnemy = enemies[selectedEnemyIndex];
        return (selectedEnemy);
    }

    public GameObject GetSelectedEnemy()
    {
        return (selectedEnemy);
    }

    public bool CheckEmpty()
    {
        bool status = true;
        if (enemies.Count != 0)
        {
            status = false;
        }
        return status;
    }
}
