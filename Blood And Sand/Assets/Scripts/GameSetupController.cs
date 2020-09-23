using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        CreatePlayer();
    }

    public void CreatePlayer()
    {
    	Debug.Log("Creating Player");
    	PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
    }

}
