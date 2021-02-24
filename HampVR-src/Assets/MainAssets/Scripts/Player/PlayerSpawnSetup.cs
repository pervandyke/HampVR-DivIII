using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = Instantiate(Resources.Load("Prefabs/PlayerModel") as GameObject, transform.position, transform.rotation);
        //player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
    }
}
