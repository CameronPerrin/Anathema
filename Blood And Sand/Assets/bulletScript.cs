using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 0.2f);
    }

    private void OnTriggerEnter(Collider collision)
    {
    	Debug.Log("Trigger");
        if(collision.gameObject.tag == "Player")
        {
        	Health hp = collision.gameObject.GetComponent<Health>();
        	hp.Damage();
            Debug.Log("COLLIDING with Bullet");
            Destroy(this.gameObject);
          
        }
    }
}
