using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathSpawnParticle : MonoBehaviour
{
    public GameObject vfx;
    public float timeBeforeDestroy;
    void Start()
    {
        spawnPart(timeBeforeDestroy);
    }

    IEnumerator spawnPart(float sec)
    {
        yield return new WaitForSeconds(sec);
        Instantiate(vfx, transform.position, transform.rotation);
    }
}
