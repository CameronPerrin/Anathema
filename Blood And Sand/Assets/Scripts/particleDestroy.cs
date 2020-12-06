using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleDestroy : MonoBehaviour
{
    
    public float duration = 0.75f;

    void Awake(){
        Destroy(this.gameObject, duration);
    }
}
