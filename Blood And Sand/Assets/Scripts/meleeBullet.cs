using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage = 0;
    public int type = 0;
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
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Corrupted_Player")
        {
            Health hp = collision.gameObject.GetComponent<Health>();
            if (hp)
            {
                hp.TakeDamage(damage, type, false);
            }
        }
        else if(collision.gameObject.tag == "Bullet");
        else
            Destroy(this.gameObject);

    }
}
