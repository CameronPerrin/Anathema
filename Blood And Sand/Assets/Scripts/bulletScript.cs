using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class bulletScript : MonoBehaviour
{
    [HideInInspector]public GameObject CurrentPlayer;
    [HideInInspector]public float dmg = 0;
    [HideInInspector]public int type;
    [HideInInspector]public bool DOT = false;

    [HideInInspector]public Rigidbody rb;
    public bool slashMeleeAttack = false;
    public bool stabMeleeAttack = false;
    public bool fastMagicAttack = false;
    public bool strongMagicAttack = false;
    public int projectileSpeed = 10;
    public bool isCrit;
    public Vector3 aim;

    // VFX
    public GameObject impactVFX;
    public float destroyTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        CurrentPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
        if(fastMagicAttack || strongMagicAttack)
            rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!fastMagicAttack && !strongMagicAttack)
            Destroy(this.gameObject, 0.2f);
        else{
            transform.LookAt(aim);
            rb.velocity = transform.forward * projectileSpeed;
            Destroy(this.gameObject, 2f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log(collision.name);
        //Debug.Log("Hit: " + collision.name); --> use this to check to see what the bullet is actually hitting
        //probably find a wayto do this without assigning a variable each time this spawns if we need to optimize the script in the future
        if ((collision.gameObject.tag == "Corrupted_Player") && (collision.gameObject != CurrentPlayer))
        {
            // VFX
            GameObject impactPoint = collision.gameObject.transform.Find("SpawnImpactVFX").gameObject;
            Instantiate(impactVFX, impactPoint.transform.position, impactPoint.transform.rotation);

        	//Health hp = collision.gameObject.GetComponent<Health>();
            if(collision.gameObject.GetComponent<Health>()){
                Debug.Log("Bullet doing damage");
                collision.gameObject.GetComponent<Health>().TakeDamage(dmg, type, DOT); 
            }
            Destroy(this.gameObject, destroyTimer);  
        }
        if ((collision.gameObject.tag == "Player") && (collision.gameObject != CurrentPlayer))
        {
        	//Health hp = collision.gameObject.GetComponent<Health>();
            if(CurrentPlayer.tag == "Corrupted_Player"){
                // VFX
                GameObject impactPoint = collision.gameObject.transform.Find("SpawnImpactVFX").gameObject;
                Instantiate(impactVFX, impactPoint.transform.position, impactPoint.transform.rotation);

                if(collision.gameObject.GetComponent<Health>()){
                    Debug.Log("Bullet doing damage");
                    collision.gameObject.GetComponent<Health>().TakeDamage(dmg, type, DOT); 
                }
            }
            Destroy(this.gameObject, destroyTimer);  
        }
        if((collision.gameObject.tag == "EnemyHitbox") && (collision.gameObject != CurrentPlayer)){
            // VFX
            if(collision.gameObject.transform.Find("SpawnImpactVFX").gameObject != null){
                GameObject impactPoint = collision.gameObject.transform.Find("SpawnImpactVFX").gameObject;
                Instantiate(impactVFX, impactPoint.transform.position, impactPoint.transform.rotation);
            }

            npcHealth hp = collision.gameObject.GetComponent<npcHealth>();
            //Debug.Log(hp);
            if(hp){
               hp.TakeDamage(dmg, type, DOT, isCrit); 
            }

            Destroy(this.gameObject,destroyTimer);
        }

        else if ((collision.gameObject.tag == "Boss") && (collision.gameObject != CurrentPlayer))
        {
            // VFX
            GameObject impactPoint = collision.gameObject.transform.Find("SpawnImpactVFX").gameObject;
            Instantiate(impactVFX, impactPoint.transform.position, impactPoint.transform.rotation);

            npcHealth hp = collision.gameObject.GetComponent<npcHealth>();
            //Debug.Log(hp);
            if (hp)
            {
                hp.TakeDamage(dmg, type, DOT, isCrit);
            }

            Destroy(this.gameObject,0);
        }

        else if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "EditorOnly" || collision.gameObject.tag == "Untagged");
        // destroy if it collides with anything else
        // else if(collision.gameObject.tag == "Untagged"){
        //     Debug.Log("Tag: " + collision.gameObject.tag);
        //     Debug.Log("Name: " + collision.gameObject.name);
        //     Destroy(this.gameObject);
        // }

        
        
    }
}
