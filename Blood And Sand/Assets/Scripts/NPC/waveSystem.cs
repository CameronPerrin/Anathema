// Author: Calvin Le
// Wave System script coordinates and controls NPC spawning in the map.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System.IO;

public class waveSystem : MonoBehaviour
{

    // Allow developers to modify add and modify wave mechanics from Inspector.
    [System.Serializable]
    public class Wave
    {
        public bool isBossWave = false;
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

    [System.Serializable]
    public class SpawnBox
    {
        public Vector3 Position;
        public Vector3 Size;
    }

    private PhotonView PV;
    public Wave[] waves;
    [SerializeField] private SpawnBox[] spawnLocations;
    public GameObject boss;
    private int nextWave = -1;
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
    private bool allWavesComplete = false;
    [SerializeField] private int townWaitTimer;

    [SerializeField] private GameObject bossSpawner;
    [SerializeField] private GameObject invisibleWall;
    [SerializeField] private GameObject teleportPosition;

    private GameObject netController;
    private List<GameObject> pObjects;

    // Chat event system
    private GameObject chat;
    private GameObject playerChat;

    bool setTimeBetweenWaves = false;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            waveCountdown = timeBetweenWaves;
            setTimeBetweenWaves = true;
        }
        else
        {
            //Destroy(this);
        }
    }


    void Update()
    {
        if(PV.IsMine){
            if(!setTimeBetweenWaves){
                waveCountdown = timeBetweenWaves;
                setTimeBetweenWaves = true;
            }
            
        // if(playerChat == null){
        //     if(playerChat = GameObject.Find("PhotonPlayer(Clone)").gameObject);
        //     else{
        //         playerChat = null;
        //     }
            
        // }
        if(playerChat==null)
        {
            var temp = GameObject.Find("PhotonPlayer(Clone)");
            if (temp != null){
                playerChat = temp.gameObject;
                //PV = playerChat.GetComponent<PhotonView>();
            }
        }
        // else{
        //     Debug.Log("[SYSTEM]: Can't find player to send chat from.");
        // }
        // if(chat == null)
        //     chat = GameObject.Find("ChatSTUFF/chatPanel/chatbox").gameObject;
        
        if (!allWavesComplete)
        {
            if (state == SpawnState.WAITING)
            {
                //Check if enemies are still alive
                if (!EnemyIsAlive())
                {

                    //Begin a new round
                    beginNewRound();
                    
                }
                else
                {
                    return;
                }
            }
            if (waveCountdown <= 0)
            {
                if (state != SpawnState.SPAWNING)
                {
                    //Start spawning wave
                    PV.RPC("sendWaveData", RpcTarget.All, nextWave);
                    StartCoroutine(EnemyDrop(waves[nextWave]));
                }
            }
            else
            {
                waveCountdown -= Time.deltaTime;
            }
        }
        }
    }

    void beginNewRound()
    {
        Debug.Log("Wave Completed!");
            //GetComponent<ChatScript>().PV.RPC("sendChat", RpcTarget.All,"", , true);
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        if(nextWave != -1){
            if(PV.IsMine)
                    //chat.GetComponent<TMP_Text>().text += $"<color=#ffc800><I>Wave completed!</I></color> \n";
                    playerChat.GetComponent<ChatScript>().PV.RPC("sendChat", RpcTarget.All,"", $"<color=#ffc800><I>Wave completed!</I></color>", true);
        }
        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            // Loop or Send all players to next scene
            Debug.Log("ALL WAVES COMPLETE!");
            if(PV.IsMine)
                //chat.GetComponent<TMP_Text>().text += $"<color=#ffc800><I>You feel dark magic in the air...</I></color>\n";
                playerChat.GetComponent<ChatScript>().PV.RPC("sendChat", RpcTarget.All,"", $"<color=#42f5e0><I>Soulgard is calling you back home... You are being teleported.</I></color>", true);
            allWavesComplete = true;

            /*
            Debug.Log("Looping Waves");
            for (int i = 0; i < waves.Length; i++)
            {
                waves[i].enemyCount = 0;
            } */
            
            PV.RPC("RPCloadingBackToTown", RpcTarget.All);
            //StartCoroutine(loadBackToTown());
        }
        else
        {
            Debug.Log("nextWave incremented");
            nextWave++;
        }
        

    }

    // Send the player back to town
    IEnumerator loadBackToTown()
    {
        Debug.Log("Loading back to town");
        while (true)
        { // This creates a never-ending loop
            yield return new WaitForSeconds(townWaitTimer);
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LeaveRoom();
            // while(PhotonNetwork.InRoom)
            //     yield return null;
            // PhotonNetwork.LoadLevel(1);
            break;
        }
    }

    [PunRPC]
    void RPCloadingBackToTown()
    {
        StartCoroutine(loadBackToTown());
    }
    bool EnemyIsAlive()
    {
        //searchCountdown is used so the function doesn't check every frame.
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("EnemyHitbox") == null && GameObject.FindGameObjectWithTag("Boss_Spawner") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator EnemyDrop(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        if(PV.IsMine)
            //chat.GetComponent<TMP_Text>().text += $"<color=#ffc800><I>{_wave.name} is starting...</I></color>\n";
            playerChat.GetComponent<ChatScript>().PV.RPC("sendChat", RpcTarget.All,"", $"<color=#ffc800><I>{_wave.name} is starting...</I></color>", true);
        state = SpawnState.SPAWNING;

        if(_wave.isBossWave)
        {
            bossSpawner.SetActive(true);
            invisibleWall.SetActive(true);
            //TeleportPlayer();
            PV.RPC("TeleportPlayer", RpcTarget.All);
        }
        else
        {
            while (_wave.enemyCount <= _wave.maxSpawnCount)
            {
                float randomEnemyRange = Random.Range(Mathf.Round(_wave.startingSpawnMin), Mathf.Round(_wave.startingSpawnMax));
                for (int k = 0; k < (int)randomEnemyRange; k++)
                {
                    // Grab random X,Y,Z position within Gizmo Cube to spawn enemy
                    int randomSpawnLocation = Random.Range(0, spawnLocations.Length);
                    Vector3 pos = spawnLocations[randomSpawnLocation].Position + new Vector3(Random.Range(-spawnLocations[randomSpawnLocation].Size.x / 2, spawnLocations[randomSpawnLocation].Size.x / 2), Random.Range(-spawnLocations[randomSpawnLocation].Size.y / 2, spawnLocations[randomSpawnLocation].Size.y / 2), Random.Range(-spawnLocations[randomSpawnLocation].Size.z / 2, spawnLocations[randomSpawnLocation].Size.z / 2));
                    //Vector3 pos = transform.localPosition + center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
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

        for(int i = 0; i < spawnLocations.Length; i++)
        {
            Gizmos.DrawCube(spawnLocations[i].Position, spawnLocations[i].Size);
        }

    }
    [PunRPC]
    public void sendWaveData(int nWave)
    {
        nextWave = nWave;
    }

    [PunRPC]
    public void TeleportPlayer()
    {
        Debug.Log("Teleporting Player to Boss Area...");
        GameObject CurrentPlayer;
        CurrentPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
        CurrentPlayer.GetComponent<CharacterController>().enabled = false;
        //CurrentPlayer.GetComponent<Rigidbody>().position = new Vector3(CurrentPlayer.transform.position.x, 10, CurrentPlayer.transform.position.z);
        CurrentPlayer.transform.position = teleportPosition.transform.position;
        CurrentPlayer.GetComponent<CharacterController>().enabled = true;
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
