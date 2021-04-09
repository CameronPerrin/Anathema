using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

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
    public float timeBetweenAOEAttacks;
    bool alreadyAttacked = false;
    bool alreadyAttackedMines = false;
    public GameObject attackPrefab;
    public GameObject shootPoint;
    public float damage = 10;

    public int numShots; 
    public int spreadAngle;
    private float atkTimer;

    private int chosenAtk;

    [SerializeField] public GameObject enemyBoss;
    private GameObject colliderTrigger;
    [SerializeField] public GameObject clone;
    private int cloneLocation;
    [SerializeField] public GameObject bosses;

    private GameObject portalController;

    private Phase phase;

    npcHealth bossHealth;

    private float searchCountdown = 1f;



    // Mines Attack Data
    private float aoeAtkTimer = 1.75f;
    [SerializeField] private GameObject AOEAttackPrefab;
    private bool minesCanAtk = false;
    //Variables to draw Gizmo Cube (Spawn Box)
    private Vector3 center;
    public Vector3 size;
    public Vector3 position;

    private void Awake()
    {
        phase = Phase.WaitingToStart;
        colliderTrigger = GameObject.Find("BossColliderTrigger");
        portalController = GameObject.Find("BossTeleportationController");
    }

    private void Start()
    {
        colliderTrigger.GetComponent<BossColliderTrigger>().OnPlayerEnterTrigger += ColliderTrigger_OnPlayerEnterTrigger;
        bossHealth = enemyBoss.GetComponent<npcHealth>();
        atkTimer = Random.Range(5, 10);

    }



    private void Update()
    {
        BossBattle_OnDamaged();
        switch (phase)
        {
            case Phase.Phase_2:
                randomAttack();
                break;
            case Phase.Phase_3:
                randomAttack();
                break;
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
                            //minesAttackSequence();
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
                        minesAttackSequence();
                        if (chosenAtk == 1)
                        {
                            AttackPlayer();

                        }
                        else
                        {
                            AttackPlayerSpiral();
                        }

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
        colliderTrigger.GetComponent<BossColliderTrigger>().OnPlayerEnterTrigger -= ColliderTrigger_OnPlayerEnterTrigger;
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
                cloneLocation = portalController.GetComponent<BossTeleportationController>().findVacantPortal();
                PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", clone.name), portalController.GetComponent<BossTeleportationController>().portals[cloneLocation].transform.position, Quaternion.identity);
                clone.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(portalController.GetComponent<BossTeleportationController>().portals[cloneLocation].transform.position);
                portalController.GetComponent<BossTeleportationController>().portals[cloneLocation].GetComponent<BossPortal>().isVacant = false;
                break;
            case Phase.Phase_2:
                phase = Phase.Phase_3;
                cloneLocation = portalController.GetComponent<BossTeleportationController>().findVacantPortal();
                PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", clone.name), portalController.GetComponent<BossTeleportationController>().portals[cloneLocation].transform.position, Quaternion.identity);
                clone.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(portalController.GetComponent<BossTeleportationController>().portals[cloneLocation].transform.position);
                portalController.GetComponent<BossTeleportationController>().portals[cloneLocation].GetComponent<BossPortal>().isVacant = false;
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
                attackHitbox.GetComponent<RangedBossBullet>().damage = damage;
                attackHitbox.GetComponent<RangedBossBullet>().type = 1;
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
            attackHitbox.GetComponent<RangedBossBullet>().damage = damage;
            attackHitbox.GetComponent<RangedBossBullet>().type = 2;
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

    private void ResetAttackMines()
    {
        alreadyAttackedMines = false;
    }


    private void minesAttackSequence()
    {
        if (alreadyAttackedMines == false)
        {
            // Spawn Mines
            Vector3 pos = position + center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
            PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", "AOE_Attack_Boss_Particle"), pos, AOEAttackPrefab.transform.rotation);



            alreadyAttackedMines = true;
            Invoke(nameof(ResetAttackMines), timeBetweenAOEAttacks);
        }
    }

    private void CheckForPlayers()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, 4f);
        foreach(Collider c in colliders)
        {
            if(c.GetComponent<PlayerMovementController>())
            {
                // Deal damage
                Debug.Log("AOE Attack Dealt Damage to Player");
                GameObject attackHitbox = Instantiate(attackPrefab, shootPoint.transform.position, transform.rotation * Quaternion.Euler(-90, 0, 0));
            }
        }
    }

    // Gizmo used to see spawn area.
    void OnDrawGizmosSelected()
    {
        //Make Spawn Cube Red
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(position + center, size);
    }


}

