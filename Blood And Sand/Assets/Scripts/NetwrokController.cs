﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetwrokController : MonoBehaviourPunCallbacks
{



	/*
	Documentation: https://doc.photonengine.com/en-us/pun/current/getting-started/pun-intro
	Scripting API: https://doc-api.photonengine.com/en/pun/v2/index.html
	*/

	void Start()
	{
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " Server!");
	}
}
