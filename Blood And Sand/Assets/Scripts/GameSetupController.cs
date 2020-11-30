using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    public void CreatePlayer()
    {
    	Debug.Log("Creating Player");
    	
        PhotonNetwork.LocalPlayer.TagObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.up, Quaternion.identity);
    }


}
