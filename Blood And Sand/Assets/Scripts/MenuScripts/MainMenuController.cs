using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private int sceneIndex;

    [SerializeField]
    private GameObject startButton;

    [SerializeField]
    private GameObject loadButton;

    [SerializeField]
    private GameObject enterWorld;

    [SerializeField]
    private GameObject inputField;

    // input field
    [SerializeField]
    private GameObject helpText;
    private GameObject pName;
    private string text;

    void Start()
    {
        inputField.GetComponent<TMP_InputField>().characterLimit = 12;
        pName = GameObject.Find("NameSaver");
        if(!pName){
            Debug.Log("CAN'T FIND OBJECT NameSaver");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        startButton.SetActive(true);
        loadButton.SetActive(false);

    }

    public void JoinTown()
    {
        // grab text from input field
        text = inputField.GetComponent<TMP_InputField>().text;
        // text has to be a minimum of 3 characters to work
        if(text.Length >= 3 && text.Length <= 12){
            helpText.GetComponent<TMP_Text>().text = $"<color=green>Username approved!</color>";
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = false;
            startButton.SetActive(false);
            enterWorld.SetActive(true);
            pName.GetComponent<playerName>().saveName(text);
            PhotonNetwork.JoinOrCreateRoom("Anathema", roomOptions, TypedLobby.Default);
        }
        else{
            //Debug.Log("You entered: " + text + ". Invalid username!");
            helpText.GetComponent<TMP_Text>().text = $"<color=red><I>Invalid username...</I></color>";
        }
        
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(sceneIndex);
    }
}
