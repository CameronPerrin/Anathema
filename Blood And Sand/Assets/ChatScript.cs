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
    public GameObject chatPanel;

    private bool isActive = false;
    //private bool isHidden = false;
    private string text;
    private string nameColor;
    private PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        chatBox = GameObject.Find("chatbox");
        chatPanel = GameObject.Find("chatPanel");
        if(chatBox == null){
            Debug.Log("Chatbox is missing, are you sure it's in the scene?");
        }
        if(chatPanel == null){
            Debug.Log("CANT FIND PANEL");
        }
        chatPanel.SetActive(true);
        PV = GetComponent<PhotonView>();
        if(!PV.IsMine){
            chatCanvas.SetActive(false);
        }
        inputBox.GetComponent<TMP_InputField>().characterLimit = 1516;

        int cNumb = Random.Range(0,4);
            if(cNumb == 0){
                nameColor = "15616D";
            }
            else if(cNumb == 1){
                nameColor = "FF7D00";
            }
            else if(cNumb == 2){
                nameColor = "78290F";
            }
            else if(cNumb == 3){
                nameColor = "001524";
            }
    }

    // Update is called once per frame
    void Update()
    {
        if(chatPanel == null){
            Debug.Log("CANT FIND PANEL");
        }
        chatPanel.SetActive(true);
        // opens input for chatting
        if(Input.GetKeyUp(KeyCode.Return) && !isActive){
            this.GetComponent<PlayerDash>().isPaused = true;
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
            text = textFilter(text);
            if(PV.IsMine && text!="")
                PV.RPC("sendChat", RpcTarget.All, PhotonNetwork.NickName, text);
            inputBox.GetComponent<TMP_InputField>().text = null;
            this.GetComponent<PlayerDash>().isPaused = false;
            this.GetComponent<PlayerMovementController>().isPaused = false;
            inputBox.SetActive(false);
            isActive = false;
        }   
    }

    [PunRPC]
    void sendChat(string name, string text)
    {

        // NEEDS TO USE RICH TEXT TO BE ABLE TO CHANGE COLOR
        chatBox.GetComponent<TMP_Text>().text += $"[<color=#{nameColor}><b>{name}</b></color>]: "+ text + "\n";
    }

    string textFilter(string textf)
    {
        string retText = "";
        textf = textf.ToLower();
        foreach (string word in textf.Split(' ')){
            if(textf.Split(' ')[0] == word);
            else
                retText += " ";
            
            if(word == "nigger" || word == "nigga" || word == "nigg" || word == "nyugah")
                retText += "ninja";
            else if(word == "fuck" || word == "fuk")
                retText += "flip";
            else if(word == "fucking" || word == "fuking")
                retText += "hecking";
            else if(word == "shit" || word == "sht")
                retText += "poop";
            else if(word == "shitting" || word == "shiting")
                retText += "poopin";
            else if(word == "hell" || word == "hel")
                retText += "heck";
            else if(word == "kkk")
                retText += "I'm insecure :(";
            else
                retText += word;
        }
        return retText;
    }

}
