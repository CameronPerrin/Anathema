using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class essenceScript : MonoBehaviour
{
    [HideInInspector]public Rigidbody rb;
    public int amount = 10;
    public GameObject pMenu;
    private bool found = false;
    public bool isCorruptedEssence = false;
    // Start is called before the first frame update
    void Start()
    {
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
            GameObject pl = collision.gameObject;
            GameObject moneyStorage = pl.transform.Find("InventoryManager/InventoryUI/InventoryCanvas/InventoryPanel/CurrencyCounter").gameObject;
            //Debug.Log(moneyStorage.name);
            moneyStorage.GetComponent<CurrencyCounter>().addMoney(amount);
            if(isCorruptedEssence){
                if(pl.GetComponent<Health>().isCorrupted == false){
                    pl.tag = "Corrupted_Player";
                    pl.GetComponent<Health>().isCorrupted = true;
                    pl.transform.Find("playerModelUnity/body").gameObject.GetComponent<Renderer>().material.color = Color.red;
                    pl.GetComponent<Health>().maxHp *= 2;
                    pl.GetComponent<Health>().health = pl.GetComponent<Health>().maxHp;
                    pl.GetComponent<Health>().physicalDefense += 10;
                    pl.GetComponent<Health>().magicDefense += 10;
                    pl.GetComponent<PlayerMovementController>().moveSpeed *= 2;   
                } 
            }
            Destroy(this.gameObject);
        }
    }
}
