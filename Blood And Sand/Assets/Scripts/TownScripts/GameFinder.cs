using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFinder : MonoBehaviourPunCallbacks
{
	private int playerCount;

	[SerializeField]
	private GameObject playerListingPrefab;

	[SerializeField]
	private Transform playersContainer;

	//[SerializeField]
	//private GameObject inputField;

	//private string roomName;

	void ClearPlayerListings()
	{
		for (int i = playersContainer.childCount - 1; i >= 0; i--)
		{
			Destroy(playersContainer.GetChild(i).gameObject);
		}
	}

	public void findGame()
    {
        Debug.Log("find a new room and join it");
		//joinorcreateroom based on player level
		//join room with the name of your player level or make another one if its full 
		//with name bieng playerlevel + "(2)"
		//roomName = inputField.GetComponent<InputField>().text;
		//PhotonNetwork.JoinOrCreateRoom(roomName);


	}

	void ListPlayers()
	{
		foreach (Player player in PhotonNetwork.PlayerList)
		{
			GameObject tempListing = Instantiate(playerListingPrefab, playersContainer);
			Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
			//tempText.text = player.NickName;
			//eventually we will use steam names for now we use player 1,2,3,4
			playerCount += 1;
			tempText.text = "player " + playerCount.ToString();
		}
	}

	//public override void OnJoinedRoom()
	//{
		//show room panel and fill it with players
		
		//if lobby has 4 players start

		//ClearPlayerListings();
		//ListPlayers();
	//}
}
