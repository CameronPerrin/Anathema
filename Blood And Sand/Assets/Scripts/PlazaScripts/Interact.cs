using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public int interactItem;

    public void interactFunction(GameObject player){
        switch (interactItem){
                case 1: 
                    Debug.Log("Mage Tower");
                    break;
                case 2:
                    Debug.Log("Blacksmith");
                    player.transform.position = new Vector3(0, -85, 0);
                    break;
                case 3:
                    Debug.Log("Armorsmith");
                    break;
                default:
                    Debug.Log("Default interact msg");
                    break;
            }
    }
}
