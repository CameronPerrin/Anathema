using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class bulletScript : MonoBehaviour
{
    public GameObject CurrentPlayer;
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
        //probably find a wayto do this without assigning a variable each time this spawns if we need to optimize the script in the future
        if((collision.gameObject.tag == "Player") && (collision.gameObject != CurrentPlayer))
        {
        	Health hp = collision.gameObject.GetComponent<Health>();
            if(hp){
               hp.TakeDamage(); 
            }
        	
            Destroy(this.gameObject);
          
        }
    }
}
