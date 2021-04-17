using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Detector detector;
    public GameObject impactVFX;
    public List<AudioClip> impactSFX;

    private bool collided;
    
    void OnCollisionEnter (Collision co) 
    {
        if (co.gameObject.tag != "Bullet" && co.gameObject.tag != "Player" && !collided)
        {
            collided = true;

            if (impactVFX != null)
            {
                ContactPoint contact = co.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;

                var hitVFX = Instantiate(impactVFX, pos, rot) as GameObject;
                var num = Random.Range (0, impactSFX.Count);
                var audioSource = hitVFX.GetComponent<AudioSource>();

                if(audioSource != null)
                    audioSource.PlayOneShot(impactSFX[num]);

                Destroy (hitVFX, 2);
            }

            if(detector != null)
                detector.DestroyAllLinks();

            Destroy(gameObject);
        }
    }
}
