using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatNPCRanged : MonoBehaviour
{

    public GameObject rayOrigin;
    private GameObject player;
    private GameObject tempObj;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;
    public GameObject attackPrefab;
    public GameObject shootPoint;

    void FixedUpdate()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 30;
        RaycastHit hit;
        if(Physics.Raycast(rayOrigin.transform.position, forward, out hit, 30)){
            if(hit.collider.tag == "Player"){ // player is detected, time to start doing stuff! (like damage)
                AttackPlayer();
                gameObject.GetComponent<mindlessFollow>().hitP = true;
                Debug.DrawRay(rayOrigin.transform.position, forward, Color.yellow);
                tempObj = hit.collider.gameObject;
            }
            else{
                gameObject.GetComponent<mindlessFollow>().hitP = false;
            }
        }
        else{
            gameObject.GetComponent<mindlessFollow>().hitP = false;
        }
        
    }

    private void AttackPlayer()
    {
        if(alreadyAttacked == false)
        {
            Debug.Log("Attacking Range");
            //attack code
            GameObject attackHitbox = Instantiate(attackPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
            //attackHitbox.transform.parent = player.transform;
            //
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        Debug.Log("ResetAttack invoked");
        alreadyAttacked = false;
    }
}
