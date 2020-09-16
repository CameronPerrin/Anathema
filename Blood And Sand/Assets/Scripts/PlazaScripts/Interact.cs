using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public int interactItem;
    public GameObject spawnLocation;

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
                default: //default
                    Debug.Log("Default interact msg");
                    break;
            }
    }
}
