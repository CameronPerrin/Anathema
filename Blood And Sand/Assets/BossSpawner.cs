using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;


// Store bosses in array
// Set bosses inactive
// Wait for boss wave
// Set bosses active



public class BossSpawner : MonoBehaviourPun
{
    public GameObject boss;
    public waveSystem waveSystem;
    private float countDown = 5f;
    private bool bossHasSpawned = false;
    private float searchCountdown = 1f;

    // Chat event system
    private GameObject playerChat;
    private GameObject chat;
    PhotonView PV;

    void Start()
    { 
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(this);
        }
        

    }
    void Update()
    {
        // if(playerChat == null){
        //     if(GameObject.Find("PhotonPlayer(Clone)") != null){
        //         playerChat = GameObject.Find("PhotonPlayer(Clone)").gameObject;
        //         PV = playerChat.GetComponent<PhotonView>();
        //     }
        // }
        if(playerChat==null)
        {
            var temp = GameObject.Find("PhotonPlayer(Clone)");
            if (temp != null){
                playerChat = temp.gameObject;
                PV = playerChat.GetComponent<PhotonView>();
            }
        }
        // else{
        //     Debug.Log("[SYSTEM]: Can't find player to send chat from.");
        // }
        // if(chat == null)
        //     chat = GameObject.Find("ChatSTUFF/chatPanel/chatbox").gameObject;

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
                if(PV.IsMine)
                    playerChat.GetComponent<ChatScript>().PV.RPC("sendChat", RpcTarget.All,"", $"<color=#ffc800><I>The Void Lord has been defeated!</I></color>", true);
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
            if(PV.IsMine)
                playerChat.GetComponent<ChatScript>().PV.RPC("sendChat", RpcTarget.All,"", $"<color=#ffc800><I>The Void Lord has been summoned!</I></color>", true);
            //chat.GetComponent<TMP_Text>().text += $"<color=#ffc800><I>The Void Lord has been summoned!</I></color> \n";
            //PhotonNetwork.Instantiate("NPCs/BossMainNPC", new Vector3(0, 0, 0), Quaternion.identity, 0);
            PhotonNetwork.InstantiateSceneObject("NPCs/BossMainNPC", new Vector3(0, 0, 0), Quaternion.identity);
            bossHasSpawned = true;
        }
    }


}
