using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

// At start, set all portals to true, spawn boss at random portal, set portal that is occupied to false;
// Once player hits boss, start timer
// After timer, hide all enemies (scale down to 0 or renderer = false)
// Set all portals to true;

public class BossMovement : MonoBehaviour
{
    private GameObject[] bosses;
    private PhotonView PV;
    public GameObject mainBoss;
    private GameObject portalController;
    public float teleportTime;
    private float teleportTimer;
    private bool hasTakenDamage;
    private bool collisionOccured;
    public bool cloneDamageTaken;
    int spawnLocation;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        portalController = GameObject.Find("BossTeleportationController");
    }

    void Start()
    { 
        teleportTimer = teleportTime;
        spawnLocation = Random.Range(0, portalController.GetComponent<BossTeleportationController>().portals.Length - 1);
        // Causes the index out of range error, will fix later
        mainBoss.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(portalController.GetComponent<BossTeleportationController>().portals[spawnLocation].transform.position);
        portalController.GetComponent<BossTeleportationController>().portals[spawnLocation].GetComponent<BossPortal>().isVacant = false;
        //portal.boss = GameObject.FindGameObjectsWithTag("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if (hasTakenDamage || cloneDamageTaken)
            {
                //Debug.Log("Boss took damage");
                if (PV.IsMine)
                {
                    PV.RPC("WaitUntilTeleport", RpcTarget.All);
                }

                //WaitUntilTeleport();
            }

            if (mainBoss.GetComponent<npcHealth>().health <= 0)
            {
                for (int i = 0; i < bosses.Length; i++)
                {
                    Destroy(bosses[i]);
                }
            }
        }
        else
        {
            return;
        }



    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collisionOccured)
        {
            return;
        }
        if(collision.gameObject.tag == "Bullet" && !hasTakenDamage)
        {
            hasTakenDamage = true;
            collisionOccured = true;

        }
    }

    [PunRPC]
    private void waitTeleport(float time)
    {
        Invoke(nameof(WaitUntilTeleport), time);
    }


    [PunRPC]
    private void WaitUntilTeleport()
    {
        teleportTimer -= Time.deltaTime;
        if (teleportTimer <= 0f)
        {
            teleportTimer = teleportTime;
            bosses = GameObject.FindGameObjectsWithTag("Boss");
            portalController.GetComponent<BossTeleportationController>().boss = GameObject.FindGameObjectsWithTag("Boss");

            /*
            for(int i = 0; i < bosses.Length; i++)
            {
                bosses[i].GetComponent<Renderer>().enabled = false;
            } */
             
            if (PV.IsMine)
            {
                PV.RPC("StartTeleportRPC", RpcTarget.All);
            }
            //StartCoroutine(Teleport());
        }
    }

    [PunRPC]
    public void StartTeleportRPC()
    {
        StartCoroutine(Teleport());
    }
    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(1);

        // Set all portals to vacant since All bosses teleports out.
        portalController.GetComponent<BossTeleportationController>().setAllPortalsVacant();

        List<int> uniqueNumbers = new List<int>();
        List<int> portalIndexes = new List<int>();

        for (int i = 0; i < portalController.GetComponent<BossTeleportationController>().portals.Length; i++)
        {
            uniqueNumbers.Add(i);
        }

        for (int i = 0; i < portalController.GetComponent<BossTeleportationController>().portals.Length; i++)
        {
            int ranNum = uniqueNumbers[Random.Range(0, uniqueNumbers.Count)];
            portalIndexes.Add(ranNum);
            uniqueNumbers.Remove(ranNum);
        }
        Debug.Log("Boss Length: " + portalController.GetComponent<BossTeleportationController>().boss.Length);
        for (int j = 0; j < portalController.GetComponent<BossTeleportationController>().boss.Length; j++)
            {
              Debug.Log("Iterating through boss length.");
              if (portalController.GetComponent<BossTeleportationController>().portals[portalIndexes[j]].GetComponent<BossPortal>().isVacant == true)
                  {
                        Debug.Log("Teleporting to portal...: " + portalIndexes[j]);
                        portalController.GetComponent<BossTeleportationController>().portals[portalIndexes[j]].GetComponent<BossPortal>().isVacant = false;

                

                        portalController.GetComponent<BossTeleportationController>().boss[j].GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(portalController.GetComponent<BossTeleportationController>().portals[portalIndexes[j]].transform.position);

            }
            }

        /*
        for (int i = 0; i < bosses.Length; i++)
        {
            bosses[i].GetComponent<Renderer>().enabled = true;
        } */
        //portalIndexes.Clear();
        hasTakenDamage = false;
        collisionOccured = false;
        cloneDamageTaken = false;
    }
}
