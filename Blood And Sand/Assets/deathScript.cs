using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
/// DEATH COMES FOR US ALL
public class deathScript : MonoBehaviourPunCallbacks
{

    public void onDeath()
	{ 
        Debug.Log("DEATH IS CALLED");

		PhotonNetwork.LeaveRoom();

	}

    public override void OnJoinedRoom()
    {
        Debug.Log("PLAYER HAS JOINED THE ROOM");
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnConnectedToMaster(){
        Debug.Log("AC/DC");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        PhotonNetwork.JoinOrCreateRoom("Anathema", roomOptions, TypedLobby.Default);
    }
}
