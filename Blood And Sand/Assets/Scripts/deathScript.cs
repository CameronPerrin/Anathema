using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
/// DEATH COMES FOR US ALL
public class deathScript : MonoBehaviourPunCallbacks
{
    public List<GameObject> playerObjects;

    /*
    private void OnDisable()
    {
        GameObject CurrentPlayer;
        CurrentPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
        killPlayer(CurrentPlayer);
    } */

    public void killPlayer(GameObject dedp)
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if(dedp == playerObjects[i]){
                playerObjects.RemoveAt(i);
            }
        }
    }

    public void onDeath()
	{ 
        Debug.Log("DEATH IS CALLED");
        //playerObjects.RemoveAt(0);
        //playerObjects[0] = null;
        PhotonNetwork.AutomaticallySyncScene = false;
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
