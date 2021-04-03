using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Interact : MonoBehaviour
{
    public int interactItem;
    public GameObject spawnLocation;
    public GameObject networkUI;
    public Camera playerCam;
    public GameObject presetLoot;

    //for case 4
    private GameObject playerBoi;

    //for weapon grab case 5
    public Transform weaponShowing;
    public GameObject pauseMenu;

    //for highlighting
    private Color startcolor;
    private bool highlight;


    void Awake(){
        startcolor = GetComponent<Renderer>().material.color;
    }


    public void interactFunction(GameObject player){
        switch (interactItem){
            case 1: // Mage tower
                Debug.Log("Mage Tower");
                player.transform.position = spawnLocation.transform.position;
                break;
            case 2: // Blacksmith
                Debug.Log("Blacksmith");
                player.transform.position = spawnLocation.transform.position;
                break;
            case 3: // Armorsmith
                Debug.Log("Armorsmith");
                player.transform.position = spawnLocation.transform.position;
                break;
            case 4: // Network UI
                Debug.Log("NetworkUI");
                networkUI.SetActive(true);
                playerBoi = PhotonNetwork.LocalPlayer.TagObject as GameObject;
                //playerBoi.transform.GetChild(0).gameObject.SetActive(false);
                playerBoi.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                break;
            case 5: //weapon grab
            	//Add feedback for when the item is purchased
                //make it so you can only use this once. rn you can spam this
                Debug.Log("Weapon Grab");
                //pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
		                // //give the weapon to the player
		                // weaponShowing.parent = playerBoi.transform.GetChild(2);
		                // //reposition it
		                // weaponShowing.position = playerBoi.transform.GetChild(2).position;
		                // weaponShowing.rotation = playerBoi.transform.GetChild(2).rotation;
                //only continue if you have the money for it

                //remove money from account
                Debug.Log("Current Money: "+ pauseMenu.GetComponent<PauseMenuController>().Money);
                Debug.Log("Current Cost: "+ weaponShowing.gameObject.GetComponent<WeaponStats>().item_value);
                int tempCurrent = pauseMenu.GetComponent<PauseMenuController>().Money;
                int tempCost = weaponShowing.gameObject.GetComponent<WeaponStats>().item_value;
                pauseMenu.GetComponent<PauseMenuController>().Money = tempCurrent - tempCost;
                Debug.Log("Money After Purchase: "+ pauseMenu.GetComponent<PauseMenuController>().Money);
                //put it in the save file
                pauseMenu.GetComponent<InventoryController>().MainHandWeapon = weaponShowing.gameObject;
                pauseMenu.GetComponent<InventoryController>().Save();

                //remove it from the pedestal
                //this can be better in the future
                Destroy(gameObject);

                break;
            case 6: //Corrupted Essence
                //pauseMenu.GetComponent<InventoryController>().MainHandWeapon = presetLoot;
                //pauseMenu.GetComponent<InventoryController>().Save();
                player.transform.Find("playerModelUnity/body").gameObject.GetComponent<Renderer>().material.color = Color.red;
                player.GetComponent<Health>().maxHp *= 2;
                player.GetComponent<Health>().health = player.GetComponent<Health>().maxHp;
                player.GetComponent<Health>().physicalDefense += 10;
                player.GetComponent<Health>().magicDefense += 10;
                player.GetComponent<PlayerMovementController>().moveSpeed *= 2;
                Destroy(this.presetLoot);
                transform.gameObject.SetActive(false);
                break;
            default: //default
                Debug.Log("Default interact message.");
                break;
        }
    }



    /// HIGHLIGHTING FUNCTIONS HERE
    public void isHighlightOn(bool check){
        if(check){
            StartCoroutine(highlightTimer());
            check = false;
        }
    }

    public IEnumerator highlightTimer(){
        //Debug.Log("Changing color to yellow!");
        GetComponent<Renderer>().material.color = Color.yellow;     //change the color to yellow
        yield return new WaitForSeconds(1);                         //wait for 3 seconds
        //Debug.Log("BACK TO GRAY");
        GetComponent<Renderer>().material.color = startcolor;       // change color back to original
    }
}
