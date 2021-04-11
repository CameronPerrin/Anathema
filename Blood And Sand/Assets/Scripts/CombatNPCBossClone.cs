using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatNPCBossClone: MonoBehaviour
{
    public GameObject rayOrigin;
    private GameObject player;
    private GameObject tempObj;

    //Attacking
    public float timeBetweenNormalAttacks;
    public float timeBetweenSpiralAttacks;
    bool alreadyAttacked = false;
    public GameObject attackPrefab;
    public GameObject shootPoint;
    private PhotonView PV;
    public LayerMask IgnoreMe;


    public int numShots; 
    public int spreadAngle;
    private float atkTimer;
    private int chosenAtk;

    npcHealth bossHealth;

    private float searchCountdown = 1f;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        atkTimer = Random.Range(5, 10);
    }

    private void Update()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            randomAttack();
            PV.RPC("SetRandomAttack", RpcTarget.All, atkTimer, chosenAtk);
        }
        //randomAttack();
    }
    void FixedUpdate()
    {
            Vector3 forward = transform.TransformDirection(Vector3.forward) * 600;
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin.transform.position, forward, out hit, 600, ~IgnoreMe))
            {
                if (hit.collider.tag == "Player")
                { // player is detected, time to start doing stuff! (like damage)
                        if(chosenAtk == 1)
                        {
                            AttackPlayer();
                        }
                        else
                        {
                            AttackPlayerSpiral();
                        }
                    gameObject.GetComponent<mindlessFollow>().hitP = true;
                    Debug.DrawRay(rayOrigin.transform.position, forward, Color.yellow);
                    tempObj = hit.collider.gameObject;
                }
                else
                {
                    gameObject.GetComponent<mindlessFollow>().hitP = false;
                }
            }
            else
            {
                gameObject.GetComponent<mindlessFollow>().hitP = false;
            } 
    }

    //[PunRPC]
    private void randomAttack()
    {
        atkTimer -= Time.deltaTime;
        if(atkTimer <=  0f)
        {
            atkTimer = Random.Range(0, 10);
            chosenAtk = Random.Range(1, 3);
        }

    }

    [PunRPC]
    public void SetRandomAttack(float attackTime, int chosenAttack)
    {
        atkTimer = attackTime;
        chosenAtk = chosenAttack;
    }

    private void AttackPlayer()
    {
        if(alreadyAttacked == false)
        {
            // Normal Attack
            Quaternion qAngle = Quaternion.AngleAxis(-numShots / 2 * spreadAngle, transform.up) * transform.rotation;
            Quaternion qDelta = Quaternion.AngleAxis(spreadAngle, transform.up);

            for (int i = 0; i < numShots; i++)
            {
                GameObject attackHitbox = Instantiate(attackPrefab, shootPoint.transform.position, qAngle);
                qAngle = qDelta * qAngle;
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenNormalAttacks);
        }
    }
    private void AttackPlayerSpiral()
    {
        if (alreadyAttacked == false)
        {

            GameObject attackHitbox = Instantiate(attackPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
            shootPoint.transform.Rotate(new Vector3(0, 1000f * Time.deltaTime , 0f));


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenSpiralAttacks);
        }

        //GameObject attackHitbox = Instantiate(attackPrefab, shootPoint.transform.position, bulDir);

    }

    private void ResetAttack()
    {
        //Debug.Log("ResetAttack invoked");
        alreadyAttacked = false;
    }


}

