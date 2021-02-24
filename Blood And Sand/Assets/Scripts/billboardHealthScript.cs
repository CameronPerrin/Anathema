using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboardHealthScript : MonoBehaviour
{
    public GameObject myCamera;

    void Start()
    {
        myCamera = GameObject.Find("Main Camera");
    }
    void Update()
    {
        transform.LookAt(transform.position + myCamera.transform.rotation * Vector3.back, myCamera.transform.rotation * Vector3.up);
    }
}
