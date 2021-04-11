using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossColliderTrigger : MonoBehaviour
{

    public event EventHandler OnPlayerEnterTrigger;

    private PhotonView PV;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!PV.IsMine)
        {
            return;
        }
        OnPlayerEnterTrigger?.Invoke(this, EventArgs.Empty);
        //https://answers.unity.com/questions/554003/check-if-player-is-colliding-with-a-trigger.html
        if (collider.gameObject.tag == "Player")
        {
            // Player inside trigger area
            Debug.Log("Player inside boss area.");
            OnPlayerEnterTrigger?.Invoke(this, EventArgs.Empty);
        } 
    }

}
