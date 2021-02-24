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
    //private GameObject inputField;

    // input field
    private string text;

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        startButton.SetActive(true);
        loadButton.SetActive(false);

    }

    public void JoinTown()
    {
        // grab text from input field
        //text = inputField.GetComponent<TMP_InputField>().text;
        // text has to be a minimum of 3 characters to work
        //if(text.Length >= 3){
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = false;
            PhotonNetwork.JoinOrCreateRoom("Anathema", roomOptions, TypedLobby.Default);
        //}
        //else{
        //    Debug.Log("You entered: " + text + ". Invalid username!");
       // }
        
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(sceneIndex);
    }

}
