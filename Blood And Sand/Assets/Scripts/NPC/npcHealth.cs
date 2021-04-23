using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public int defense = 0;
    public int magicDefense = 0;
    public float dotTimer = 0.5f;
    public float dropChance = 0.2f;
    public float corruptionDropChance = 0.5f;
    public Image worldHealthBar;		// HP images for worldspace overlay
    public Image redWorldHealthBackdrop;
    public GameObject wHp; 				// canvas world
    public GameObject bloodVFX;
    public GameObject lootDrop;
    public GameObject corruptionDrop;
    //public GameObject bloodSpotInstLocation;


   // public Camera playerCamera;
    public GameObject FloatingTextPrefab;
    public float dmgTemp;
    public float dmg; // Placeholder damage for when we add in weapons


    public event EventHandler OnDamaged;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        health = maxHp;
    }

    // This function is called in another script to tell this script when to take damage.
    public void TakeDamage(float dmage, int type, bool dot)
    {
        if(type == 1 || type == 6){          // Physical damage
            dmg = dmage - defense;
            PV.RPC("Damage", RpcTarget.All, dmg, false);    
        }
        else if(type == 2 || type == 7){     // Magic damage
            dmg = dmage - magicDefense;
            PV.RPC("Damage", RpcTarget.All, dmg, false);    
        }
        if(dot){
            //Debug.Log("DOT DAMAGE!");
            dmgTemp = dmage;
            dmg = (dmage - defense) / 4;
            InvokeRepeating ("PhysicalDOTDmg", 0f, dotTimer);
        }
    }

    public void PhysicalDOTDmg()
    {
        if(dmgTemp > 0){
            //Debug.Log("Bleeding for " + dmg + " damage!");
            PV.RPC("Damage", RpcTarget.All, dmg, true);
            dmgTemp -= dmg;
        }
        else{
            this.CancelInvoke("PhysicalDOTDmg");
        }
    }

    public void Update()
    {
        if(health/maxHp != redWorldHealthBackdrop.fillAmount)
            redWorldHealthBackdrop.fillAmount = Mathf.Lerp(redWorldHealthBackdrop.fillAmount, health/maxHp, Time.deltaTime * 4);
    }


    // this stuff sends info to all network
    [PunRPC]
    public void Damage(float dmage, bool dotOn)
    {
        health -= dmage;
        ShowFloatingText();
        if(dotOn)
            Instantiate(bloodVFX, transform.position, Quaternion.identity); // spawn blood vfx
        //Instantiate(bloodVFX, bloodSpotInstLocation.transform.position, Quaternion.identity); // spawn blood vfx
            if(health <= 0){
                float rand = UnityEngine.Random.Range(0.01f, 1.0f);
                if(rand <= corruptionDropChance)
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Corrupted Essence"), new Vector3 (transform.position.x, 10, transform.position.z), Quaternion.identity, 0);
                    //Instantiate(corruptionDrop, new Vector3 (transform.position.x, 6, transform.position.z), Quaternion.identity);
                    
                if(rand <= dropChance)
                    Instantiate(lootDrop, new Vector3 (transform.position.x, 10, transform.position.z), Quaternion.identity);
                Destroy(this.gameObject);
            }  
        worldHealthBar.fillAmount = health/maxHp;
        
        // Check if floating text exists and if health is over 0 so that damage text does not spawn at hp 0
            //if(FloatingTextPrefab)
            //{
                
            //}
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
        go.GetComponent<TextMesh>().text = dmg.ToString();
    }
}
