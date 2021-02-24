using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopMove : MonoBehaviour
{
    public GameObject NPC;
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player"){
            NPC.GetComponent<mindlessFollow>().stopMove = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == "Player"){
            NPC.GetComponent<mindlessFollow>().stopMove = false;
        }
    }
}
