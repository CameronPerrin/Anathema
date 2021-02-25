using Photon.Pun;
using System.IO;
using UnityEngine;

public class NPCTestSpawn : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        SpawnNPC();
    }

    public void SpawnNPC()
    {
    	Debug.Log("SpawnNPC");


    	if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", "SimpleMeleeNPC"), transform.position, Quaternion.identity);
            PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", "SimpleRangedNPC"), transform.position, Quaternion.identity);
        }
    	//PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "SimpleMeleeNPC"), transform.position, Quaternion.identity);
    	//PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "SimpleRangedNPC"), transform.position, Quaternion.identity);
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SimpleMeleeNPC"), transform.position, Quaternion.identity);
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SimpleRangedNPC"), transform.position, Quaternion.identity);
    }


}
