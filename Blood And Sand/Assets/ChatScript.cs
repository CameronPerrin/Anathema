using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class ChatScript : MonoBehaviourPun
{

    public GameObject inputBox;
    public GameObject chatBox;
    public GameObject chatCanvas;

    private bool isActive = false;
    //private bool isHidden = false;
    private string text;
    private PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        chatBox = GameObject.Find("chatbox");
        PV = GetComponent<PhotonView>();
        if(!PV.IsMine){
            chatCanvas.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // opens input for chatting
        if(Input.GetKeyUp(KeyCode.Return) && !isActive){
            this.GetComponent<PlayerMovementController>().isPaused = true;
            //chatCanvas.SetActive(true);
            inputBox.SetActive(true);
            inputBox.GetComponent<TMP_InputField>().ActivateInputField(); 
            isActive = true;
            //isHidden = false;

        }

        // closes input for chatting and sends msg
        else if(Input.GetKeyUp(KeyCode.Return) && isActive){
            text = inputBox.GetComponent<TMP_InputField>().text;
            //chatCanvas.SetActive(true);
            if(PV.IsMine)
                PV.RPC("sendChat", RpcTarget.All, PhotonNetwork.NickName, text);
            inputBox.GetComponent<TMP_InputField>().text = null;
            this.GetComponent<PlayerMovementController>().isPaused = false;
            inputBox.SetActive(false);
            isActive = false;
        }   
    }

    [PunRPC]
    void sendChat(string name, string text)
    {
        chatBox.GetComponent<TMP_Text>().text += "[" + name +"]: "+ text + "\n";
    }

}
