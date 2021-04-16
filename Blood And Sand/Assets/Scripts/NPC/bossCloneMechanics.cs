

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

// When Boss is taken "damage" - Change color of prefab
// Notify main boss that "damage" is taken - Interact with Boss Movement.
        // Set bool variable to bossmovement update
//

public class bossCloneMechanics : MonoBehaviour
{

    public GameObject bossClone;
    private GameObject mainBoss;


    private void Start()
    {
        // Change to BossMainNPC(Clone) later
        mainBoss = GameObject.Find("BossMainNPC(Clone)");
        //mainBoss = GameObject.Find("BossMainNPC");
    }

    private void Update()
    {
        if(!mainBoss)
        {
            Destroy(bossClone);
        } 
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Bullet")
        {
            StartCoroutine(onDamaged());
        }
    }

    IEnumerator onDamaged()
    {
        //Change Color of Boss
        var cloneRenderer = bossClone.GetComponent<Renderer>();

        bossClone.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.magenta);

        yield return new WaitForSeconds(.5f);

        bossClone.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.green);

        //Notify Main Boss that damage is taken on clone.
        mainBoss.GetComponent<BossMovement>().cloneDamageTaken = true;
    }
}
