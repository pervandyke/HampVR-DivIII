using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDestroyer : MonoBehaviour
{

    private void Start()
    {
        EnemyManager.enemyManager.AddEnemy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            EnemyManager.enemyManager.RemoveEnemy(gameObject);
            if (Global.global.selectedTarget == gameObject)
            {
                Global.global.selectedTarget = null;
            }
            Destroy(gameObject);
        }
    }
}
