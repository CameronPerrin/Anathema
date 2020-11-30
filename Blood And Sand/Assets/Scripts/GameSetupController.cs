using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
    public Transform SpawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    public void CreatePlayer()
    {
    	Debug.Log("Creating Player");
    	
        PhotonNetwork.LocalPlayer.TagObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), SpawnPoint.position, Quaternion.identity);
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000);
    }


}
