using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public int interactItem;
    public GameObject spawnLocation;
    public GameObject networkUI;
    public Camera playerCam;

    //for case 4 and 5
    private GameObject playerBoi;

    //for weapon grab case 5
    public Transform weaponShowing;


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
                playerBoi = GameObject.FindGameObjectWithTag("Player");
                //playerBoi.transform.GetChild(0).gameObject.SetActive(false);
                playerBoi.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                break;
            case 5: //weapon grab
                Debug.Log("Weapon Grab");
                playerBoi = GameObject.FindGameObjectWithTag("Player");
                //give the weapon to the player
                weaponShowing.parent = playerBoi.transform.GetChild(2);
                //reposition it
                weaponShowing.position = playerBoi.transform.GetChild(2).position;
                weaponShowing.rotation = playerBoi.transform.GetChild(2).rotation;
                //put it in the save file

                //remove money from account and save file based on the weaponshowing.value
                break;
            default: //default
                Debug.Log("Default interact msg");
                break;
        }
    }
}
