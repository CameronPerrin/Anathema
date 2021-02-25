using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class mindlessFollow : MonoBehaviour
{

    public bool followPlayer = false;
    public bool stopMove = false;
    public bool isRangedNPC = false;
    public bool hitP = false;
    public float moveSpeed = 0.02f;
    public List<Player> playerInScene;

    private Vector3 playerPos;
    private GameObject player;
    private Rigidbody rb;
    private bool isTargetPlayer = false;
    

    private GameObject netController;
    private List<GameObject> pObjects;
    private List<float> distList = new List<float>();
    private int pCounter;
    float temp, min;

    // Navmesh stuff
    [SerializeField]
    Transform _destination;
    UnityEngine.AI.NavMeshAgent _navMeshAgent;
    

    void Start()
    {
        netController = GameObject.Find("TheReaper");
        if(netController){
            pObjects = netController.GetComponent<deathScript>().playerObjects;
        }

        rb = GetComponent<Rigidbody>();
        _navMeshAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        
        //for()        
        
    }

    void FixedUpdate()
    {
        if(!isTargetPlayer){
            int cIndex = 0;
            if(pObjects !=null){
            foreach (GameObject gamers in pObjects){
                pCounter++;
                Vector3 npos = new Vector3 (transform.position.x, 0, transform.position.z);
                Vector3 ppos = new Vector3 (gamers.transform.position.x, 0, gamers.transform.position.z);
                
                distList.Add((npos - ppos).magnitude);
            }

            
            for(int i = 0; i < pCounter; ++i){
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
            player = pObjects[cIndex];
            min = 0;
            pCounter = 0;
            cIndex = 0;
            isTargetPlayer = true;
            }
        }
        //player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(player.name);
        //if(player)
        
        //else{
           // Debug.Log("NPC: Can't find a player to look at?");
            //isTargetPlayer = false;
        //}
            
        //Debug.Log("Player found at position: " + player.transform.position);
        //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed);
        //rb.MovePosition(player.transform.position + new Vector3(3f,0f,0f) * moveSpeed * Time.fixedDeltaTime);
        if(_navMeshAgent == null && isTargetPlayer){
            Debug.Log("No nav mesh agent is attached to " + gameObject.name);
        }
        else{
            SetDestination();
        }        
    }

    private void SetDestination()
    {
        if(player){
            if(!isRangedNPC){
                Vector3 targetVector = player.transform.position;
                transform.LookAt(targetVector);
                _navMeshAgent.SetDestination(targetVector);
            }
            else{
                if(hitP){
                    _navMeshAgent.SetDestination(transform.position);
                }
                else{
                    Vector3 targetVector = player.transform.position;
                    transform.LookAt(targetVector);
                    _navMeshAgent.SetDestination(targetVector);
                }
            }
        }
        else{
            Debug.Log("Player not found to for SetDestination()");
            isTargetPlayer = false;
        }
    }

    // Update is called once per frame
    
}
