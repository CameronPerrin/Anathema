using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon;
using UnityEngine.SceneManagement;

public class Combat : MonoBehaviour
{

	private PhotonView PV;
	public GameObject attackPrefab;
	public GameObject shootPoint;
    public GameObject CurrentPlayer;
    
	void Start()
	{
		PV = GetComponent<PhotonView>();
        CurrentPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0) && SceneManager.GetActiveScene().buildIndex != 1)
    	{
    		if(PV.IsMine)
    		{
    			Attack();
    		}
    	}
	}

    public void Attack()
    {

    	PV.RPC("RpcShoot", RpcTarget.All);
    }

    [PunRPC]
    void RpcShoot()
    {
    	GameObject attackHitbox = Instantiate(attackPrefab, shootPoint.transform.position, Quaternion.identity);
        //attackHitbox.GetComponent<Rigidbody>().velocity = CurrentPlayer.transform.GetChild(1).GetComponent<Rigidbody>().velocity;
        attackHitbox.transform.parent = CurrentPlayer.transform;
    }
}
