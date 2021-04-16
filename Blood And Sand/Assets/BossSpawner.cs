using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    private float searchCountdown = 1f;

    void Start()
    { 
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(this);
        }

    }
    void Update()
    {
            if (!bossHasSpawned)
            {
                SpawnBossAfterCountdown();
            }

            if (!BossIsAlive())
            {
                Destroy(this.gameObject);
            }
    }

    bool BossIsAlive()
    {
        //searchCountdown is used so the function doesn't check every frame.
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f && bossHasSpawned)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Boss") == null)
            {
                return false;
            }
        }
        return true;
    }

    void SpawnBossAfterCountdown()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0f)
        {
            Debug.Log("Boss is now spawning!");
            //PhotonNetwork.Instantiate("NPCs/BossMainNPC", new Vector3(0, 0, 0), Quaternion.identity, 0);
            PhotonNetwork.InstantiateSceneObject("NPCs/BossMainNPC", new Vector3(0, 0, 0), Quaternion.identity);
            bossHasSpawned = true;
        }
    }


}
