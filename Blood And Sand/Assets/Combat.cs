using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon;

public class Combat : MonoBehaviour
{

	private PhotonView PV;
	public GameObject attackPrefab;
	public GameObject shootPoint;
    
	void Start()
	{
		PV = GetComponent<PhotonView>();
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
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
    	Instantiate(attackPrefab, shootPoint.transform.position, Quaternion.identity);
    }
}
