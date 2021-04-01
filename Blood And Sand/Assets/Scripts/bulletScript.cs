﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class bulletScript : MonoBehaviour
{
    [HideInInspector]public GameObject CurrentPlayer;
    [HideInInspector]public float dmg = 0;
    [HideInInspector]public int type;
    [HideInInspector]public bool DOT = false;

    [HideInInspector]public Rigidbody rb;
    public bool slashMeleeAttack = false;
    public bool stabMeleeAttack = false;
    public bool fastMagicAttack = false;
    public bool strongMagicAttack = false;
    public int projectileSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Spawned with " + dmg + " damage.");
        CurrentPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
        if(fastMagicAttack || strongMagicAttack)
            rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!fastMagicAttack && !strongMagicAttack)
            Destroy(this.gameObject, 0.2f);

        else{
            rb.velocity = transform.forward * projectileSpeed;
            Destroy(this.gameObject, 5f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if((collision.gameObject.tag == "EnemyHitbox") && (collision.gameObject != CurrentPlayer)){
            npcHealth hp = collision.gameObject.GetComponent<npcHealth>();
            //Debug.Log(hp);
            if(hp){
               hp.TakeDamage(dmg, type, DOT); 
            }

            Destroy(this.gameObject);
        }

        if ((collision.gameObject.tag == "Boss") && (collision.gameObject != CurrentPlayer))
        {
            npcHealth hp = collision.gameObject.GetComponent<npcHealth>();
            //Debug.Log(hp);
            if (hp)
            {
                hp.TakeDamage(dmg, type, DOT);
            }

            Destroy(this.gameObject);
        }

        //probably find a wayto do this without assigning a variable each time this spawns if we need to optimize the script in the future
        if ((collision.gameObject.tag == "Player") && (collision.gameObject != CurrentPlayer))
        {
        	Health hp = collision.gameObject.GetComponent<Health>();
            if(hp){
               //hp.TakeDamage(dmg); 
            }
        	
            
            Destroy(this.gameObject);
          
        }

        
    }
}
