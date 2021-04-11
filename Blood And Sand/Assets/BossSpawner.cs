using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Store bosses in array
// Set bosses inactive
// Wait for boss wave
// Set bosses active


public class BossSpawner : MonoBehaviour
{
    public GameObject boss;
    public waveSystem waveSystem;
    private float countDown = 5f;
    private bool bossHasSpawned = false;

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(this);
        }

    }
    void Update()
    {
        if(!bossHasSpawned)
        {
            SpawnBossAfterCountdown();
        }
    }

    void SpawnBossAfterCountdown()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0f)
        {
            Debug.Log("Boss is now spawning!");
            PhotonNetwork.Instantiate("NPCs/BossMainNPC", new Vector3(0, 0, 0), Quaternion.identity, 0);
            bossHasSpawned = true;
        }
        else
        {
            Debug.Log("Boss spawning in " + countDown + " seconds!");
        }
    }


}
