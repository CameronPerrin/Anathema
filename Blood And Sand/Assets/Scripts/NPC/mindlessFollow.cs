﻿/*###########################################    
* #     [Original author: Josh Soran]       #
* #     [Contributors: Calvin Lee]          #
* ###########################################
*/
// Lasted Edited: 3/7/2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class mindlessFollow : MonoBehaviour
{
    private PhotonView PV;
    public bool followPlayer = false;
    public bool stopMove = false;
    public bool isRangedNPC = false;
    public bool isBossNPC = false;
    public bool hitP = false;
    public float moveSpeed = 0.02f;
    public List<Player> playerInScene;

    private Vector3 playerPos;
    private GameObject player;
    private Rigidbody rb;
    private bool isTargetPlayer = false;
    
    // Components used to find the shortest distance to a player
    private GameObject netController;
    private List<GameObject> pObjects;
    private List<float> distList;
    float temp, min;
   
    //Vector3 direction;
    // Navmesh stuff
    [SerializeField]
    Transform _destination;
    UnityEngine.AI.NavMeshAgent _navMeshAgent;


    void Start()
    {
        // Networking
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();

        // Grab the "Reaper" to find the player list
        netController = GameObject.Find("TheReaper");
        if(netController){
            // set new list as player list that we grabbed
            pObjects = netController.GetComponent<deathScript>().playerObjects;
        }

        // Navmesh components
        _navMeshAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        // Only enable navmesh if MASTER CLIENT.
        if(!PhotonNetwork.IsMasterClient){
            _navMeshAgent.enabled = false;
        }            
    }

    // FixedUpdate() instead of Update() because of NPC HPbar orientation issues.
    void FixedUpdate()
    {
        List<float> distList = new List<float>();
        // [CONTINUED] Navmesh components, resetting and re-enabling if the ownership (Master or Client) gets transfered over after player death
        if(!PhotonNetwork.IsMasterClient){
            _navMeshAgent.enabled = false;
        }
        else{
            _navMeshAgent.enabled = true;
        }


        /****************************************************************************************************************
        *       -[NPC TARGETTING SYSTEM]-                                                                               *
        * The main goal when writing this was to have any NPC with the script attached find the nearest player to them  *
        * by comparing the distances from this NPC to each of the players, and selecting the closest one.               *
        * We're also making sure this loop doesn't run forever, and that only the Master client calls it.               *
        *****************************************************************************************************************
        */
        if(!isTargetPlayer && PhotonNetwork.IsMasterClient){
            //Debug.Log("[1] - Begin player finding...");
            // cIndex is in charge of finding the index position of the player on the player list that has the shortest distance.
            int cIndex = 0;
            int playersFound = 0;
            // Making sure the list isn't empty.
            if(pObjects !=null){
                foreach (GameObject gamers in pObjects){
                    playersFound++;
                    // NPC position
                    Vector3 npos = new Vector3 (transform.position.x, 0, transform.position.z);
                    // Current player from player list position
                    Vector3 ppos = new Vector3 (gamers.transform.position.x, 0, gamers.transform.position.z);
                    // Add the magnitude of the difference of the two distances to a new list.
                    distList.Add((npos - ppos).magnitude);
                    //Debug.Log("[2] - " + gamers.transform.Find("Canvas_WS_Overlay/name").gameObject.GetComponent<TMP_Text>().text + " is found...");
            }
            //Debug.Log("[3] - Total of: " + playersFound + " players found...");
            /*
            * Cycle through the entire list using a simple find "minimum" algorithm. Afterwards,
            * every time the new "minimum" is set, set the "cIndex" to the current index in the loop.
            * This makes it so that everytime a new minimum is found we are updating the position to 
            * the latest position found.
            */
            //Debug.Log("[4] - Distances on distance list: " + distList.Count + "...");
            for(int i = 0; i < distList.Count; i++){
                if(i == 0){
                    min = distList[0];
                    cIndex = i;
                }
                temp = distList[i];
                if(temp < min){
                    cIndex = i;
                    min = temp;
                }
            }
            //Debug.Log("[5] - Distance is calculated!");

            // Finally we set the player object equal the the player found at "cIndex", and reset everything back to 0.
            //Debug.Log("[6] - current index is: " + cIndex);
            player = pObjects[cIndex];
            //Debug.Log("[7] - " + player.transform.Find("Canvas_WS_Overlay/name").gameObject.GetComponent<TMP_Text>().text + " is the target.");
            min = 0;
            cIndex = 0;
            // This enables isTargetPlayer to true so that this doesn't needlessly loop.
            isTargetPlayer = true;
            }
            
        }
        // [OLD] --> player = GameObject.FindGameObjectWithTag("Player");

        // Only look at player if Master client, this part of what fixes rotation issues in multiplayer.
        if(PhotonNetwork.IsMasterClient){
            try{
                transform.LookAt(player.transform);
            }
            catch (Exception e){
                //Debug.Log("Is target player: " + isTargetPlayer);
				Debug.Log("NPCs trying to LOOK AT players, but cannot find any.");
			}
        }

        // Error if navAgent isnt found.
        if(_navMeshAgent == null && !isTargetPlayer){
            Debug.Log("No nav mesh agent is attached to " + gameObject.name);
        }
        else{
            if(PhotonNetwork.IsMasterClient)
            {
                // Call the function that makes the navAgent go after the player.
                try{
                    SetDestination();
                }
                catch(Exception e){ // Destroy object if error
                    Debug.Log("NPC CAN'T FIND TERRAIN... SELF-DESTRUCTING!");
                    Destroy(this.gameObject);
                }
            }
        }        
    }


    /********************************************************************************
    *       -[NAVMESH AGENT SYSTEM]-                                                *
    * This function sets the destination of the navmesh agent towards the player.   *
    * It also differentiates between whether it is a ranged NPC or a melee NPC.     *
    *********************************************************************************
    */
    private void SetDestination()
    {
        if(player){
            // MELEE NPC
            if(!isRangedNPC){
                // Begin movement towards player.
                Vector3 targetVector = player.transform.position;
                _navMeshAgent.SetDestination(targetVector);
            }
            // RANGED NPC
            else if(isBossNPC)
            {
                Vector3 targetVector = player.transform.position;
                // -----------  If Boss NPC, do nothing. Might change this later. -------
                return;
            }
            else{
                // Checking to make sure RANGED NPC can SEE the player.
                if(hitP){
                    _navMeshAgent.isStopped = true;
                }
                // Begin movement towards player.
                else{
                    _navMeshAgent.isStopped = false;
                    Vector3 targetVector = player.transform.position;
                    _navMeshAgent.SetDestination(targetVector);
                }
            }
        }
        else{
            Debug.Log("Player not found for SetDestination()");
            isTargetPlayer = false;
        }
    }
}
