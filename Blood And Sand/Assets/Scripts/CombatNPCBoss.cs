using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatNPCBoss: MonoBehaviour
{

    public enum Phase
    {
        WaitingToStart,
        Phase_1,
        Phase_2,
        Phase_3,
    }

    public GameObject rayOrigin;
    private GameObject player;
    private GameObject tempObj;

    //Attacking
    public float timeBetweenNormalAttacks;
    public float timeBetweenSpiralAttacks;
    bool alreadyAttacked = false;
    public GameObject attackPrefab;
    public GameObject shootPoint;

    public int numShots; 
    public int spreadAngle;
    private float atkTimer;
    private int chosenAtk;

    [SerializeField] private GameObject enemyBoss;
    [SerializeField] private BossColliderTrigger colliderTrigger;
    [SerializeField] private GameObject clone_1;
    [SerializeField] private GameObject clone_2;
    [SerializeField] private GameObject bosses;

    public BossTeleportationController portalController;

    private Phase phase;

    npcHealth bossHealth;

    private float searchCountdown = 1f;

    private void Awake()
    {
        phase = Phase.WaitingToStart;
    }

    private void Start()
    {
        colliderTrigger.OnPlayerEnterTrigger += ColliderTrigger_OnPlayerEnterTrigger;
        bossHealth = enemyBoss.GetComponent<npcHealth>();
        atkTimer = Random.Range(5, 10);

        clone_1.SetActive(false);
        clone_2.SetActive(false);
    }



    private void Update()
    {
        BossBattle_OnDamaged();
        switch (phase)
        {
            case Phase.Phase_2:
                randomAttack();
                break;
        }

        if(enemyBoss.GetComponent<npcHealth>().health <= 0)
        {
            Destroy(bosses);
        }
    }
    void FixedUpdate()
    {
            Vector3 forward = transform.TransformDirection(Vector3.forward) * 30;
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin.transform.position, forward, out hit, 30))
            {
                if (hit.collider.tag == "Player")
                { // player is detected, time to start doing stuff! (like damage)
                switch (phase)
                    {
                        case Phase.Phase_1:
                            AttackPlayer();
                            break;
                        case Phase.Phase_2:
                            if(chosenAtk == 1)
                        {
                            AttackPlayer();
                        }
                        else
                        {
                            AttackPlayerSpiral();
                        }
                            break;
                        case Phase.Phase_3:
                            //AttackPlayer();
                            break;
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

    private void ColliderTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e)
    {
        StartBattle();
        colliderTrigger.OnPlayerEnterTrigger -= ColliderTrigger_OnPlayerEnterTrigger;
    }    




    private void StartBattle()
    {
        StartNextStage();
    }


    private void randomAttack()
    {
        atkTimer -= Time.deltaTime;
        if(atkTimer <=  0f)
        {
            atkTimer = Random.Range(0, 10);
            chosenAtk = Random.Range(1, 3);
            //Debug.Log("Chosen Attack: " + chosenAtk);

        }

    }
    private void BossBattle_OnDamaged()
    {

        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            //Boss took damage
            switch (phase)
            {
                case Phase.Phase_1:
                    if (bossHealth.health <= (bossHealth.maxHp * 0.75))
                    {
                        //Debug.Log("Stage 1 Boss Health: " + bossHealth.health);
                        // Boss health under 75%
                        StartNextStage();
                    }
                    break;
                case Phase.Phase_2:
                    if (bossHealth.health <= (bossHealth.maxHp * 0.25))
                    {
                        //Debug.Log("Stage 2 Boss Health: " + bossHealth.health);
                        // Boss health under 25%
                        StartNextStage();
                    }
                    break;
                case Phase.Phase_3:
                    if (enemyBoss.GetComponent<npcHealth>().health <= 0)
                    {
                        Destroy(bosses);
                    }
                    break;
            }
        }

    }

    private void StartNextStage()
    {
        switch (phase)
        {
            case Phase.WaitingToStart:
                phase = Phase.Phase_1;
                break;
            case Phase.Phase_1:
                phase = Phase.Phase_2;
                Debug.Log("SPAWNING CLONES!!!!!!!!!!!!!!!");
                clone_1.SetActive(true);
                int cloneLocation_1 = portalController.findVacantPortal();
                //clone_1.transform.position = portalController.portals[cloneLocation_1].transform.position;
                clone_1.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(portalController.portals[cloneLocation_1].transform.position);
                portalController.portals[cloneLocation_1].GetComponent<BossPortal>().isVacant = false;
                break;
            case Phase.Phase_2:
                phase = Phase.Phase_3;
                clone_2.SetActive(true);
                int cloneLocation_2 = portalController.findVacantPortal();
                clone_2.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(portalController.portals[cloneLocation_2].transform.position);
                //clone_2.transform.position = portalController.portals[cloneLocation_2].transform.position;
                portalController.portals[cloneLocation_2].GetComponent<BossPortal>().isVacant = false;
                //////////     Enable mines here


                //////////
                break;
        }
        Debug.Log("Starting next phase: " + phase);
        Debug.Log("Boss Health: " + bossHealth.health);
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

