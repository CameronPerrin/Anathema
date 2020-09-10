using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   //used to make the equipped gameObject a child of the player prefab
    [HideInInspector]
    public GameObject spawned;

    private PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        PV = this.transform.parent.GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            GameObject playerLoadoutController = GameObject.Find("PlayerLoadoutPassthrough");
            spawned = Instantiate(playerLoadoutController.GetComponent<PlayerLoadoutController>().equipped, new Vector3(0, 0, 1), Quaternion.identity);
            //spawned.transform.parent = player.transform;
            spawned.transform.parent = this.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
