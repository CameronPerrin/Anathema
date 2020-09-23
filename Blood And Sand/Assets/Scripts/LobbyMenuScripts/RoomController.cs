using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RoomController : MonoBehaviourPunCallbacks
{

	[SerializeField]
	private int multiPlayerSceneIndex;

	[SerializeField]
	private int townSceneIndex;

	[SerializeField]
	private GameObject lobbyPanel;
	[SerializeField]
	private GameObject roomPanel;

	[SerializeField]
	private GameObject startButton;

	[SerializeField]
	private Transform playersContainer;
	[SerializeField]
	private GameObject playerListingPrefab;

	[SerializeField]
	private Text roomNameDisplay;
	[SerializeField]
	private GameObject lobbyObject;

	void ClearPlayerListings()
	{
		for (int i = playersContainer.childCount - 1; i >= 0; i--)
		{
			Destroy(playersContainer.GetChild(i).gameObject);
		}
	}

	void ListPlayers()
	{
		foreach (Player player in PhotonNetwork.PlayerList)
		{
			GameObject tempListing = Instantiate(playerListingPrefab, playersContainer);
			Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
			tempText.text = player.NickName;
		}
	}

	public void CreatePlayer()
	{
		Debug.Log("Creating Player");
		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
	}

	public override void OnJoinedRoom()
	{
        if (!lobbyObject.GetComponent<LobbyController>().inAnathema)
        {
			roomPanel.SetActive(true);
			lobbyPanel.SetActive(false);
			roomNameDisplay.text = PhotonNetwork.CurrentRoom.Name;
			if (PhotonNetwork.IsMasterClient)
			{
				startButton.SetActive(true);
			}
			else
			{
				startButton.SetActive(false);
			}
			ClearPlayerListings();
			ListPlayers();
		}
		else if (lobbyObject.GetComponent<LobbyController>().inAnathema)
        {
			StartGame();
		}

	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		ClearPlayerListings();
		ListPlayers();
	}
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		ClearPlayerListings();
		ListPlayers();
		if(PhotonNetwork.IsMasterClient)
		{
			startButton.SetActive(true);
		}
	}


	public void StartGame()
	{
		if(PhotonNetwork.IsMasterClient)
		{
			if (!lobbyObject.GetComponent<LobbyController>().inAnathema)
			{
				
				PhotonNetwork.CurrentRoom.IsOpen = false;
				PhotonNetwork.LoadLevel(multiPlayerSceneIndex);
			}
			else if (lobbyObject.GetComponent<LobbyController>().inAnathema)
			{
				Debug.Log("starting GAme!!! lolzors");
				PhotonNetwork.LoadLevel(townSceneIndex);
				//run start button function then creat player
				//CreatePlayer();
			}

		}
	}

	IEnumerator rejoinLobby()
	{
		yield return new WaitForSeconds(1);
		PhotonNetwork.JoinLobby();
	}

	public void BackOnClick()
	{
		lobbyPanel.SetActive(true);
		roomPanel.SetActive(false);
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LeaveLobby();
		StartCoroutine(rejoinLobby());
	}
}
