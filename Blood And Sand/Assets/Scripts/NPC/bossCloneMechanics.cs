

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
    public GameObject mainBoss;


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

        bossClone.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.cyan);

        //Notify Main Boss that damage is taken on clone.
        mainBoss.GetComponent<BossMovement>().cloneDamageTaken = true;
    }
}
