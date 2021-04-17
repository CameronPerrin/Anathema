using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class playerNameDisplayer : MonoBehaviourPun
{
    public TextMeshProUGUI nameDisplay;
    private PhotonView PV;
    
    private GameObject pNameObj;
    private string playerName;
    
    void Start()
    {   
        PV = GetComponent<PhotonView>();
        //pNameObj = GameObject.Find("NameSaver");
        if(!pNameObj){
            Debug.Log("CAN'T FIND OBJECT NameSaver");
        }
        //playerName = pNameObj.GetComponent<playerName>().pName;
        if(PV.IsMine)
            PV.RPC("sendName", RpcTarget.All, PhotonNetwork.NickName);
    }

    void Update()
    {
        if(PV.IsMine)
            PV.RPC("sendName", RpcTarget.All, PhotonNetwork.NickName);
    }

    // NOTE: If we want to display player names AFTER they enter a scene, we need to
    // use OnPhotonSerializeView component for networking. But since players are joining
    // at the same time, there's no point as of now. OnPhotonSerializeView updates
    // 10 times per second, and for something that doesn't change, it's a waste.
    [PunRPC]
    void sendName(string name)
    {
        nameDisplay.text = name;
    }

    
}
