using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mindlessFollow : MonoBehaviour
{

    public bool followPlayer = false;
    public bool stopMove = false;
    public float moveSpeed = 0.02f;

    private Vector3 playerPos;
    private GameObject player;
    private Rigidbody rb;

    // Navmesh stuff
    [SerializeField]
    Transform _destination;
    UnityEngine.AI.NavMeshAgent _navMeshAgent;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _navMeshAgent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //transform.LookAt(player.transform);
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
            Vector3 targetVector = player.transform.position;
            _navMeshAgent.SetDestination(targetVector);
        }
        else{
            Debug.Log("Player not found to for SetDestination()");
        }
    }

    // Update is called once per frame
    
}
