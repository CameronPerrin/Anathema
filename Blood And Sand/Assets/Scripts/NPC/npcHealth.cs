using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon;
using UnityEngine.SceneManagement;

public class npcHealth : MonoBehaviour
{
    // Start is called before the first frame update
	private PhotonView PV;
    public float maxHp = 100f;
    public float health;
    public Image worldHealthBar;		// HP images for worldspace overlay
    public GameObject wHp; 				// canvas world
    //public GameObject bloodVFX;
    //public GameObject bloodSpotInstLocation;


    void Start()
    {
        PV = GetComponent<PhotonView>();
        health = maxHp;
    }

    // This function is called in another script to tell this script when to take damage.
    public void TakeDamage()
    {
        Debug.Log("Step 2: TakeDamage() function called");
        if(PV.IsMine){
            PV.RPC("Damage", RpcTarget.All);
        }
        
    }


    // this stuff sends info to all network
    [PunRPC]
    public void Damage()
    {
        Debug.Log("Step 3: Damage() function called");
        health -= 10;
        //Instantiate(bloodVFX, bloodSpotInstLocation.transform.position, Quaternion.identity); // spawn blood vfx
            if(health <= 0){
                Destroy(this.gameObject);
            }  
        worldHealthBar.fillAmount = health/maxHp;
    }


    // I'm not sure if we need this funcion for the health to work, but I'm leaving it in just in case.
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
