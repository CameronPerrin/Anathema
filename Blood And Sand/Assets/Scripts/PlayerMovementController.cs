using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviourPun
{
	public float moveSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
    		transform.Translate(moveSpeed * Time.deltaTime, 0, 0, Space.World);
    	}
    }

    // Update is called once per frame
    void Update()
    {
    
    	if(photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
    		return;
    	}

        if (Input.GetKey("d"))
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0, Space.World);
        if (Input.GetKey("a"))
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0, Space.World);
        if (Input.GetKey("w"))
            transform.Translate(0, 0, moveSpeed * Time.deltaTime, Space.World);
        if (Input.GetKey("s"))
            transform.Translate(0, 0, -moveSpeed * Time.deltaTime, Space.World);
    }
}
