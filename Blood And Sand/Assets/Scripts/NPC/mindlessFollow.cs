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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(followPlayer && !stopMove){
            transform.LookAt(player.transform);
            //Debug.Log("Player found at position: " + player.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed);
        }

    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player"){
            followPlayer = true;
            player = collision.gameObject;
        }
    }
    
    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == "Player"){
            followPlayer = false;
        }
    }
}
