using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class bulletScript : MonoBehaviour
{
    public GameObject CurrentPlayer;
    public float dmg = 0;
    public int type;
    public bool DOT = false;
    // Start is called before the first frame update
    void Start()
    {
        CurrentPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 0.2f);
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
