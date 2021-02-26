using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class mindlessFollow : MonoBehaviourPunCallbacks, IPunObservable
{
    private PhotonView PV;
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
    //Vector3 direction;
    // Navmesh stuff
    [SerializeField]
    Transform _destination;
    UnityEngine.AI.NavMeshAgent _navMeshAgent;
    

    void Start()
    {
        PV = GetComponent<PhotonView>();
        netController = GameObject.Find("TheReaper");
        if(netController){
            pObjects = netController.GetComponent<deathScript>().playerObjects;
        }

        rb = GetComponent<Rigidbody>();
        _navMeshAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        _navMeshAgent.updateRotation = false;

        //direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        //for()        
        
    }

    void FixedUpdate()
    {
        if(!isTargetPlayer){
            Debug.Log("NPC: TRYING TO FIND PLAYER");
            int cIndex = 0;
            if(pObjects !=null){
            foreach (GameObject gamers in pObjects){
                pCounter++;
                Vector3 npos = new Vector3 (transform.position.x, 0, transform.position.z);
                Vector3 ppos = new Vector3 (gamers.transform.position.x, 0, gamers.transform.position.z);
                
                distList.Add((npos - ppos).magnitude);
            }

            for(int i = 0; i < distList.Count; ++i){
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
            //Debug.Log(cIndex);
            player = pObjects[cIndex];
            
            min = 0;
            //pCounter = 0;
            cIndex = 0;
            isTargetPlayer = true;
            }
        }
        //player = GameObject.FindGameObjectWithTag("Player");

        if(PhotonNetwork.IsMasterClient)
            transform.LookAt(player.transform);
        //if(player)
        //else{
           // Debug.Log("NPC: Can't find a player to look at?");
            //isTargetPlayer = false;
        //}
        //Debug.Log("Player found at position: " + player.transform.position);
        //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed);
        //rb.MovePosition(player.transform.position + new Vector3(3f,0f,0f) * moveSpeed * Time.fixedDeltaTime);
        if(_navMeshAgent == null && !isTargetPlayer){
            Debug.Log("No nav mesh agent is attached to " + gameObject.name);
        }
        else{
            SetDestination();
        }        
    }

    // [PunRPC]
    // private bool targetSet(bool isTarg){
    //     isTargetPlayer = isTarg;
    //     return isTargetPlayer;
    // }


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
            isTargetPlayer = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting){
            stream.SendNext(transform.rotation);
            stream.SendNext(transform.position);
        }
        else{
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
            this.transform.position = (Vector3)stream.ReceiveNext();
        }
    }
    
}
