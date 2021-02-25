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
    private bool isTargetPlayer;
    private int []distList;
    private GameObject netController;


    // Navmesh stuff
    [SerializeField]
    Transform _destination;
    UnityEngine.AI.NavMeshAgent _navMeshAgent;
    

    void Start()
    {
        netController = GameObject.Find("TheReaper");
        if(netController){
            netController.GetComponent<deathScript>().playerObjects.Add(gameObject);
        }

        rb = GetComponent<Rigidbody>();
        _navMeshAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        
        for()        
        
    }

    void FixedUpdate()
    {
        if(!isTargetPlayer){
            

            int temp, max;
            for(int i = 0; i < 4; i++){
                if(i == 0){

                }
            }
        }
        player = GameObject.FindGameObjectWithTag("Player");

        transform.LookAt(player.transform);
        //Debug.Log("Player found at position: " + player.transform.position);
        //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed);
        //rb.MovePosition(player.transform.position + new Vector3(3f,0f,0f) * moveSpeed * Time.fixedDeltaTime);
        if(_navMeshAgent == null){
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
                _navMeshAgent.SetDestination(targetVector);
            }
            else{
                if(hitP){
                    _navMeshAgent.SetDestination(transform.position);
                }
                else{
                    Vector3 targetVector = player.transform.position;
                    _navMeshAgent.SetDestination(targetVector);
                }
            }
        }
        else{
            Debug.Log("Player not found to for SetDestination()");
        }
    }

    // Update is called once per frame
    
}
