using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPortal : MonoBehaviour
{
    public bool isVacant;

    private Vector3 center;
    private Vector3 size = new Vector3(1,1,1);

    void Start()
    {
        isVacant = true;
    }

    void OnDrawGizmosSelected()
    {
        //Make Spawn Cube Red
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.localPosition + center, size);
    }

}
