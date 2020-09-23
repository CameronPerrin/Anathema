using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public int interactItem;
    public GameObject spawnLocation;
    //[HideInInspector]
    public GameObject LobbyPanel;
    //[HideInInspector]
    public GameObject LobbyPanelController;



    public void interactFunction(GameObject player)
    {
        switch (interactItem)
        {
            case 1: // Mage tower
                Debug.Log("Mage Tower");
                player.transform.position = spawnLocation.transform.position;
                break;
            case 2: // Blacksmith
                Debug.Log("Blacksmith");
                player.transform.position = spawnLocation.transform.position;
                break;
            case 3: // Armorsmith
                Debug.Log("Armorsmith");
                player.transform.position = spawnLocation.transform.position;
                break;
            case 4: // MatchMaking Panels
                Debug.Log("lobby tings");
                //LobbyPanel = GameObject.Find("Lobby");
                //LobbyPanelController = GameObject.Find("LobbyControllerObject");
                LobbyPanel.SetActive(true);
                LobbyPanelController.GetComponent<LobbyController>().inAnathema = false;
                Destroy(GameObject.FindWithTag("Player"));
                Cursor.lockState = CursorLockMode.None;
                //GameObject.FindWithTag("Player").GetComponent<PlayerMovementController>().disabled = true;
                //GameObject.FindWithTag("MainCamera").GetComponent<Camera>().GetComponent<MouseLock>().disabled = true;
                break;
            default: //default
                Debug.Log("Default interact msg");
                break;
        }
    }
}
