using Photon.Pun;
using System.IO;
using UnityEngine;

public class NPCTestSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpawnNPC();
    }

    public void SpawnNPC()
    {
    	Debug.Log("SpawnNPC");
    	
        PhotonNetwork.LocalPlayer.TagObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SimpleMeleeNPC"), transform.position, Quaternion.identity);
        PhotonNetwork.LocalPlayer.TagObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SimpleRangedNPC"), transform.position, Quaternion.identity);
    }


}
