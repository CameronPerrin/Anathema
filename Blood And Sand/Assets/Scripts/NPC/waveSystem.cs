// Author: Calvin Le
// Wave System script coordinates and controls NPC spawning in the map.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class waveSystem : MonoBehaviour
{

    // Allow developers to modify add and modify wave mechanics from Inspector.
    [System.Serializable]
    public class Wave
    {
        public string name;
        public Spawn[] enemies;
        public int maxSpawnCount;
        //[HideInInspector]
        public int enemyCount;
        public float timeBetweenSpawns;
        public float startingSpawnMin;
        public float startingSpawnMax;
        public float spawnMultiplier = 1;
    }

    public Wave[] waves;
    public GameObject boss;
    private int nextWave = 0;
    public bool hasBossWave;
    public BossTeleportationController portal;
    [HideInInspector] public bool isBossDead;

    // Used to show state of wave system
    public enum SpawnState { SPAWNING, WAITING, COUNTING };
    
    // Sets starting wait time and wait time between waves. Default = 30 Seconds
    public float timeBetweenWaves = 30f;
    public float waveCountdown;
    private float searchCountdown = 1f;
    public SpawnState state = SpawnState.COUNTING;

    //Variables to draw Gizmo Cube (Spawn Box)
    private Vector3 center;
    public Vector3 size;




    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            waveCountdown = timeBetweenWaves;
        }
        else
        {
            Destroy(this);
        }

    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            //Check if enemies are still alive
            if(!EnemyIsAlive())
            {

                //Begin a new round
                beginNewRound();
            }
            else
            {
                return;
            }
        }
        if(waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                //Start spawning wave
                StartCoroutine(EnemyDrop(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }

    }

    void beginNewRound()
    {
        Debug.Log("Wave Completed!");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;

            //state = SpawnState.WAITING;
            /// Hold until boss wave completed
            if (hasBossWave)
            {
                PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", boss.name), portal.portals[0].transform.position, Quaternion.identity);
                hasBossWave = false;
            }
            ///
            Debug.Log("ALL WAVES COMPLETE! Looping...");
            for(int i = 0; i < waves.Length; i++)
            {
                waves[i].enemyCount = 0;
            }



            // Multipliers goes here

        }
        else
        {
            Debug.Log("nextWave incremented");
            nextWave++;
        }

    }

    bool EnemyIsAlive()
    {
        //searchCountdown is used so the function doesn't check every frame.
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0f)
        {
            searchCountdown = 1f;

            if (GameObject.FindGameObjectWithTag("Boss") != null)
            {
                return true;
            }


            if (GameObject.FindGameObjectWithTag("EnemyHitbox") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator EnemyDrop(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        while (_wave.enemyCount <= _wave.maxSpawnCount)
        {
            float randomEnemyRange = Random.Range(Mathf.Round(_wave.startingSpawnMin), Mathf.Round(_wave.startingSpawnMax + 1));
            for (int k = 0; k < randomEnemyRange; k++)
            {
                // Grab random X,Y,Z position within Gizmo Cube to spawn enemy
                Vector3 pos = transform.localPosition + center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
                int i = Random.Range(0, 100);
                // Iterate through enemies array
                for (int j = 0; j < _wave.enemies.Length; j++)
                {
                    // Checks if i falls between min and max probability range of enemy[j]
                    if (i >= _wave.enemies[j].minProbabilityRange && i <= _wave.enemies[j].maxProbabilityRange)
                    {
                        // Spawn enemy
                        PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", _wave.enemies[j].spawnObject.name), pos, Quaternion.identity);
                        break;
                    }
                }
            }
            _wave.enemyCount += (int)randomEnemyRange;

            // Multiply min and max spawn rage by multiplier.
            _wave.startingSpawnMin *= _wave.spawnMultiplier;
            _wave.startingSpawnMax *= _wave.spawnMultiplier;

            yield return new WaitForSeconds(_wave.timeBetweenSpawns);
        }
        // All enemies are spawned for the wave and waits until all enemies are killed.
        state = SpawnState.WAITING;
        yield break;
    }


    // Gizmo used to see spawn area.
    void OnDrawGizmosSelected()
    {
        //Make Spawn Cube Red
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.localPosition + center, size);
    }
}


// Allows developer to customize which object and probability rate to spawn
[System.Serializable]
public class Spawn
{
    public GameObject spawnObject;
    public int minProbabilityRange = 0;
    public int maxProbabilityRange = 0;
}
