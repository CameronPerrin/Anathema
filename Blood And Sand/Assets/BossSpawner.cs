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
    private float searchCountdown = 3f;


    // Start is called before the first frame update
    void Start()
    {
        boss.SetActive(false);

    }

    void Update()
    {
        if(!BossIsAlive())
        {
            waveSystem.isBossDead = true;
        }
    }

    bool BossIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Boss") == null)
            {
                return false;
            }
        }
        return true;
    }


}
