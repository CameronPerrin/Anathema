using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
    public Transform SpawnPoint;

    private GameObject pNameObj;
    // Start is called before the first frame update
    void Start()
    {
        pNameObj = GameObject.Find("NameSaver");
        CreatePlayer();
    }

    public void CreatePlayer()
    {
    	Debug.Log("Creating Player");
    	
        PhotonNetwork.LocalPlayer.TagObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), SpawnPoint.position, SpawnPoint.rotation);
        PhotonNetwork.NickName = pNameObj.GetComponent<playerName>().pName;
    }


}
