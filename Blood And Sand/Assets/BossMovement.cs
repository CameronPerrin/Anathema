using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// At start, set all portals to true, spawn boss at random portal, set portal that is occupied to false;
// Once player hits boss, start timer
// After timer, hide all enemies (scale down to 0 or renderer = false)
// Set all portals to true;

public class BossMovement : MonoBehaviour
{
    public GameObject bosses;
    public GameObject mainBoss;
    public BossTeleportationController portal;
    public float teleportTime;
    private float teleportTimer;
    private bool hasTakenDamage;
    private bool collisionOccured;
    public bool cloneDamageTaken;
    int spawnLocation;

    private void OnEnable()
    {
        spawnLocation = Random.Range(0, portal.portals.Length - 1);
        mainBoss.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(portal.portals[spawnLocation].transform.position);
        portal.portals[spawnLocation].GetComponent<BossPortal>().isVacant = false;
    }


    void Start()
    { 
        teleportTimer = teleportTime;
        //portal.boss = GameObject.FindGameObjectsWithTag("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTakenDamage || cloneDamageTaken)
        {
            WaitUntilTeleport();
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

    private void WaitUntilTeleport()
    {
        teleportTimer -= Time.deltaTime;
        if (teleportTimer <= 0f)
        {
            teleportTimer = teleportTime;
            portal.boss = GameObject.FindGameObjectsWithTag("Boss");
            bosses.transform.localScale = new Vector3(0, 0, 0);
            StartCoroutine(Teleport());
        }
    }

    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(1);

        // Set all portals to vacant since All bosses teleports out.
        portal.setAllPortalsVacant();

        List<int> uniqueNumbers = new List<int>();
        List<int> portalIndexes = new List<int>();

        for (int i = 0; i < portal.portals.Length; i++)
        {
            uniqueNumbers.Add(i);
        }

        for (int i = 0; i < portal.portals.Length; i++)
        {
            int ranNum = uniqueNumbers[Random.Range(0, uniqueNumbers.Count)];
            portalIndexes.Add(ranNum);
            uniqueNumbers.Remove(ranNum);
        }
        Debug.Log("Boss Length: " + portal.boss.Length);
        for (int j = 0; j < portal.boss.Length; j++)
            {
              Debug.Log("Iterating through boss length.");
              if (portal.portals[portalIndexes[j]].GetComponent<BossPortal>().isVacant == true)
                  {
                        Debug.Log("Teleporting to portal...: " + portalIndexes[j]);
                        portal.portals[portalIndexes[j]].GetComponent<BossPortal>().isVacant = false;

                        bosses.transform.localScale = new Vector3(1, 1, 1);

                        portal.boss[j].GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(portal.portals[portalIndexes[j]].transform.position);
                  }
            }
        portalIndexes.Clear();
        hasTakenDamage = false;
        collisionOccured = false;
        cloneDamageTaken = false;
    }
}
