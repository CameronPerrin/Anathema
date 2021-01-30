using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggro : MonoBehaviour
{
    public GameObject NPC;

    void Start()
    {
        NPC.GetComponent<mindlessFollow>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player"){
            NPC.GetComponent<mindlessFollow>().followPlayer = true;
        }
    }
}
