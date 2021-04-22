using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class backOutScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject networkUI;

    public void backOut()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        GameObject playerBoi = PhotonNetwork.LocalPlayer.TagObject as GameObject;
        playerBoi.GetComponent<pauseHierarchyScript>().pauseThings(false);
        playerBoi.GetComponent<pauseHierarchyScript>().blockMainMenu = false;
    }
}
