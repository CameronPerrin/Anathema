using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class CombatNPCMelee : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask whatIsPlayer;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;
    public GameObject attackPrefab;
    public GameObject shootPoint;

    // States
    public float attackRange;
    public bool playerInSightRange, playerInAttackRange;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (playerInAttackRange)
        {
            AttackPlayer();
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        //agent.Stop();
        agent.SetDestination(transform.position);

        if(alreadyAttacked == false)
        {
            //attack code
            GameObject attackHitbox = Instantiate(attackPrefab, shootPoint.transform.position, Quaternion.identity);
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

