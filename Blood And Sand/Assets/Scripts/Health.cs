using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon;


public class Health : MonoBehaviourPunCallbacks
{

    // Start is called before the first frame update
	private PhotonView PV;
    public float maxHp = 100f;
    public float health;
    public Image overlayHealthBar; 		// HP images for screenspace overlay
    public Image worldHealthBar;		// HP images for worldspace overlay
    public GameObject oHp; 				// canvas overlay
    public GameObject wHp; 				// canvas world

    public GameObject deathObj;
    public GameObject bloodVFX;
    public GameObject bloodSpotInstLocation;

    void Start()
    {
    	PV = GetComponent<PhotonView>();
        health = maxHp;
        if(PV.IsMine)
    	{
    		wHp.SetActive(false);
    	}
    	else{
    		oHp.SetActive(false);
    	}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        if(PV.IsMine){
            PV.RPC("Damage", RpcTarget.All);
        }
    }

    [PunRPC]
    public void Damage()
    {
        health -= 10;
        Instantiate(bloodVFX, bloodSpotInstLocation.transform.position, Quaternion.identity); // spawn blood vfx
        if(PV.IsMine) {
            if(health <= 0){
                GameObject.Find("TheReaper").GetComponent<deathScript>().onDeath(); // Find obj, find script, call function
            }  
        }
        overlayHealthBar.fillAmount = health/maxHp;
        worldHealthBar.fillAmount = health/maxHp;
        

    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting)
        {
            //this is my player! >:(
            stream.SendNext(health);
        }

        else if(stream.IsReading)
        {
            //everyone else
            health = (float)stream.ReceiveNext();
        }
    }

    
}
