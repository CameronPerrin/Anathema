using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossBullet : MonoBehaviour
{
    public Rigidbody rb;
    private Collider collider;
    public int projectileSpeed = 10;
    public float damage = 0;
    public int type = 0;
    private Vector3 des;
    public GameObject VFXhit;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = transform.forward * projectileSpeed;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player"|| collision.gameObject.tag == "Corrupted_Player")
        {
            Instantiate(VFXhit, transform.position, Quaternion.identity);
            Health hp = collision.gameObject.GetComponent<Health>();
            if (hp)
            {
                hp.TakeDamage(damage, type, false);
            }
            
        }
        if (collision.gameObject.tag == "Untagged")
        {

            des = new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z + 1);
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, des, .1f);
        }
        if (collision.gameObject.tag == "Invisible_Wall")
        {
            Destroy(this.gameObject);
        }

        /*else if(collision.gameObject.tag == "Bullet");
        else 
            Destroy(this.gameObject); */
        //Destroy(this.gameObject);
    }

}
