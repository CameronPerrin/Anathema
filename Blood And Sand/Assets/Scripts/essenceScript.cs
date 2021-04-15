using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class essenceScript : MonoBehaviourPun
{
    [HideInInspector]public Rigidbody rb;
    public int essenceAmount = 10;
    public GameObject pMenu;
    private bool found = false;
    public bool isCorruptedEssence = false;
    private PhotonView PV;
    public Inventory inventory;
    GameObject pl;
    GameObject moneyStorage;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.up * 10;
    }

    // Update is called once per frame
    void Update()
    {
        // make it rotate to give it some life :)
        transform.Rotate (0,50*Time.deltaTime,0);

        if(!found)
            pMenu = GameObject.Find("PauseMenu");
        else if(pMenu != null)
            found = true;
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Corrupted_Player"){
            
            // pl = collision.gameObject;
            // //Debug.Log(pl.name);
            // //moneyStorage = pl.transform.Find("InventoryManager/InventoryUI/InventoryCanvas/InventoryPanel/CurrencyCounter").gameObject;
            // //Debug.Log(pl.name+" "+moneyStorage.name);
            // //Debug.Log(moneyStorage.name);
            // //Debug.Log(PV.IsMine);
            // if(PV.IsLocal){
            //     //Debug.Log(PV.IsMine);
            //     inventory.Money += essenceAmount;
            //     if(isCorruptedEssence){
            //         if(pl.GetComponent<Health>().isCorrupted == false){
            //             pl.tag = "Corrupted_Player";
            //             pl.GetComponent<Health>().isCorrupted = true;
            //             pl.transform.Find("playerModelUnity/body").gameObject.GetComponent<Renderer>().material.color = Color.red;
            //             pl.GetComponent<Health>().maxHp *= 2;
            //             pl.GetComponent<Health>().health = pl.GetComponent<Health>().maxHp;
            //             pl.GetComponent<Health>().physicalDefense += 10;
            //             pl.GetComponent<Health>().magicDefense += 10;
            //             pl.GetComponent<PlayerMovementController>().moveSpeed *= 2;   
            //         } 
            //     }
                
            // }
            //Destroy(this.gameObject);
        }  
    }
}
