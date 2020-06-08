using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private GameObject startButton;
	[SerializeField]
	private GameObject cancelButton;
	[SerializeField]
	private int roomSize;

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
		startButton.SetActive(true);
	}

	public void QuickStart()
	{
		startButton.SetActive(false);
		cancelButton.SetActive(true);
		PhotonNetwork.JoinRandomRoom();
		Debug.Log("Attempting to join a room...");
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("Failed to join a room!");
		CreateRoom();
	}

	void CreateRoom()
	{
		Debug.Log("Creating a room now...");
		int randomRoomNumber = Random.Range(0, 10000);
		RoomOptions roomOps = new RoomOptions() {IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
		PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
		Debug.Log("Created room: " + randomRoomNumber);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.Log("Failed to create room... error: " + returnCode);
		Debug.Log("Trying again...");
		CreateRoom();
	}

	public void QuickCancel()
	{
		cancelButton.SetActive(false);
		startButton.SetActive(true);
		PhotonNetwork.LeaveRoom();
	}
}
