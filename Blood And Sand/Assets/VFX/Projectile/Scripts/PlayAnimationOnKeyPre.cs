using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnKeyPre : MonoBehaviour
{

    public GameObject mainProjectile;
    public ParticleSystem mainParticleSystem;

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            mainProjectile.SetActive(true);
        }

        if (mainParticleSystem.IsAlive() == false)
        {
            mainProjectile.SetActive(false);
        }
    }
}
