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
            if (Global.global.leftSelectedTarget == gameObject)
            {
                Global.global.leftSelectedTarget = null;
            }
            else if (Global.global.rightSelectedTarget == gameObject)
            {
                Global.global.rightSelectedTarget = null;
            }
            Destroy(gameObject);
        }
    }
}
