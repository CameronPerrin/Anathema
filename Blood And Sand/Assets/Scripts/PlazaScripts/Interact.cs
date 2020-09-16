using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public int interactItem;

    public void interactFunction(){
        switch (interactItem){
                case 1: 
                    Debug.Log("Mage Tower");
                    break;
                case 2:
                    Debug.Log("Blacksmith");
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
