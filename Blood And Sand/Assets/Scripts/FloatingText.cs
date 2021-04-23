using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    public float DestroyTime = .25f;
    public Vector3 Offset = new Vector3(0, 2, 0);
    public Vector3 RandomizeIntensity = new Vector3(0.5f, 0, 0);
    public Animator textAnim;


    void Start()
    {
        textAnim = GetComponent<Animator>();
        textAnim.speed = Random.Range(0.9f, 3f);
        // When instantiated, delete after DestroyTime.
        Destroy(gameObject, DestroyTime);
        // Controls the height at which damage text spawns
        transform.localPosition += Offset;
        // Controls the X,Y,Z position at which the text spawns.
        transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
            Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
            Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));
    }
}
