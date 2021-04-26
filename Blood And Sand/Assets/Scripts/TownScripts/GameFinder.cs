using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameFinder : MonoBehaviourPunCallbacks
{
	private int numberOfFails;
	private int playerCount;
	private GameObject tempListing;
	[SerializeField]
	private TMP_Text tempText;

	[SerializeField]
	private GameObject playerListingPrefab;

	[SerializeField]
	private Transform playersContainer;

	[SerializeField]
	private GameObject roomPanel;

	[SerializeField]
	private GameObject findButton;

	[SerializeField]
	private GameObject startButton;

	//[SerializeField]
	//private GameObject inputField;

	//private string roomName;

	void ClearPlayerListings()
	{
		for (int i = playersContainer.childCount - 1; i >= 0; i--)
		{
			Destroy(playersContainer.GetChild(i).gameObject);
		}
		// for (int i = 0; i < playersContainer.childCount; i++){
		// 	Destroy(playersContainer.GetChild(i).gameObject);
		// }
	}

	void ListPlayers()
	{
		foreach (Player player in PhotonNetwork.PlayerList)
		{
			playerCount++;
			tempListing = Instantiate(Resources.Load("Prefabs/PlayerListing"), playersContainer.position-new Vector3(0, (playerCount*55), 0), Quaternion.identity, playersContainer) as GameObject;
			tempText = tempListing.transform.GetChild(0).GetComponent<TMP_Text>();
			//Debug.Log(tempListing);
			//Debug.Log(tempText);
			tempText.text = player.NickName;
			//eventually we will use steam names for now we use player and a number

		}
		if(playerCount == 1){
			if(PhotonNetwork.IsMasterClient) {
				startButton.SetActive(true);
			}
		}
		playerCount = 0;
	}

	public void findGame()
    {
        Debug.Log("find a new room and join it");
        //leaveroom
        PhotonNetwork.LeaveRoom();
		//joinorcreateroom based on player level
			//done in OnLeftRoom()
		//join room with the name of your player level or make another one if its full 
		//with name bieng playerlevel + "(2)"
		//roomName = inputField.GetComponent<InputField>().text;
		//PhotonNetwork.JoinOrCreateRoom(roomName);


	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
		RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 4;
		PhotonNetwork.JoinOrCreateRoom("Combat", roomOptions, TypedLobby.Default);
	}

	//on fail join room creat another room with the same room options and name combat + numberOfFails
	public override void OnJoinRoomFailed(short returnCode, string message) {
		numberOfFails++;
        Debug.Log("You failed to join room. Attemping to join room combat"+numberOfFails.ToString());
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 4;
		PhotonNetwork.JoinOrCreateRoom("Combat"+numberOfFails.ToString(), roomOptions, TypedLobby.Default);
    }

	public override void OnJoinedRoom()
    {
		roomPanel.SetActive(true);
		findButton.SetActive(false);
		ClearPlayerListings();
		ListPlayers();

        //PhotonNetwork.LoadLevel(4);
    }

	public void StartGame()
	{
		//eventually make it so auto starts after like 10 sec
		if(PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.CurrentRoom.IsOpen = false;
			PhotonNetwork.LoadLevel(2);
		}
	}

    public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		ClearPlayerListings();
		ListPlayers();
	}

    public override void OnPlayerLeftRoom(Player newPlayer)
	{
		ClearPlayerListings();
		ListPlayers();
	}


	//public override void OnJoinedRoom()
	//{
		//show room panel and fill it with players
		
		//if lobby has 4 players start

		//ClearPlayerListings();
		//ListPlayers();
	//}

	
}
