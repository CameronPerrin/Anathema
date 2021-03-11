using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon;
using UnityEngine.SceneManagement;

public class npcHealth : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
	private PhotonView PV;
    public float maxHp = 100f;
    public float health;
    public Image worldHealthBar;		// HP images for worldspace overlay
    public Image redWorldHealthBackdrop;
    public GameObject wHp; 				// canvas world
    public GameObject bloodVFX;
    //public GameObject bloodSpotInstLocation;


   // public Camera playerCamera;
    public GameObject FloatingTextPrefab;
    public int damage; // Placeholder damage for when we add in weapons


    public event EventHandler OnDamaged;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        health = maxHp;
    }

    // This function is called in another script to tell this script when to take damage.
    public void TakeDamage()
    {
        if(PV.IsMine){
            PV.RPC("Damage", RpcTarget.All);

            
        }
        
    }

    public void Update()
    {
        if(health/maxHp != redWorldHealthBackdrop.fillAmount)
            redWorldHealthBackdrop.fillAmount = Mathf.Lerp(redWorldHealthBackdrop.fillAmount, health/maxHp, Time.deltaTime * 4);
    }


    // this stuff sends info to all network
    [PunRPC]
    public void Damage()
    {
        health -= 10;
        Instantiate(bloodVFX, this.transform.position, Quaternion.identity); // spawn blood vfx
        //Instantiate(bloodVFX, bloodSpotInstLocation.transform.position, Quaternion.identity); // spawn blood vfx
            if(health <= 0){
                Destroy(this.gameObject);
            }  
        worldHealthBar.fillAmount = health/maxHp;
        
        // Check if floating text exists and if health is over 0 so that damage text does not spawn at hp 0
            if(FloatingTextPrefab)
            {
                ShowFloatingText();
            }
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
    

    // Spawn in damage text and makes sure that the text rotation is facing the camera
    // ** This might be changed later because text does not show sometimes in multiplayer session. **
    void ShowFloatingText()
    {
        var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        go.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        //go.transform.LookAt(Camera.main.transform);
        go.GetComponent<TextMesh>().text = damage.ToString();
    }
}
