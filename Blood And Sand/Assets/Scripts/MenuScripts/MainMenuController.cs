using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private string sceneIndex;

    [SerializeField]
    private GameObject startButton;

    [SerializeField]
    private GameObject loadButton;

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        startButton.SetActive(true);
        loadButton.SetActive(false);

    }

    public void JoinTown()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        PhotonNetwork.JoinOrCreateRoom("Anathema", roomOptions, TypedLobby.Default);
        
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }

}
