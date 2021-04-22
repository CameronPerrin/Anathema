using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class ToggleActiveWithKeyPress : MonoBehaviourPun
{
    [SerializeField] private KeyCode inventoryKeyCode = KeyCode.None;
    [SerializeField] private KeyCode equipmentKeyCode = KeyCode.None;
    [SerializeField] private KeyCode firstHotBarSlotKeyCode = KeyCode.None;
    [SerializeField] private KeyCode secondHotBarSlotKeyCode = KeyCode.None;
    [SerializeField] private KeyCode thirdHotBarSlotKeyCode = KeyCode.None;
    [SerializeField] private GameObject inventoryToToggle = null;
    [SerializeField] private GameObject equipmentToToggle = null;
    [SerializeField] private GameObject playerMouseLock = null;

    [HideInInspector] public bool isPaused = false;

    private GameObject[] hotbarSlots = null;

    public GameObject pauseScript;
    PhotonView PV;

    private void Start()
    {
        //pauseScript = GameObject.Find("PhotonPlayer(Clone)").gameObject;
        PV = GetComponent<PhotonView>();
        hotbarSlots = GameObject.FindGameObjectsWithTag("Hotbar_Slot");
        playerMouseLock = GameObject.Find("PhotonPlayer(Clone)");

        inventoryToToggle.transform.localScale = new Vector3(0, 0, 0);
        equipmentToToggle.transform.localScale = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        if(PV.IsMine){
            var tempScript = playerMouseLock.GetComponent<pauseHierarchyScript>();

            if(tempScript.closeAllPanelsButMenu){
                inventoryToToggle.transform.localScale = new Vector3(0, 0, 0);
                equipmentToToggle.transform.localScale = new Vector3(0, 0, 0);
                tempScript.inventoryPause = false;
                tempScript.charPanelPause = false;
            }

        if (Input.GetKeyDown(inventoryKeyCode))
        {
            
            //inventoryToToggle.SetActive(!inventoryToToggle.activeSelf);
            

            if (inventoryToToggle.transform.localScale == new Vector3(0, 0, 0))
            {
                if(tempScript.menuPause || tempScript.chatPause);
                else{
                    inventoryToToggle.transform.localScale = new Vector3(1, 1, 1);
                    tempScript.setPauseActive(2);
                }
            }
            else
            {
                if(tempScript.chatPause);
                else{
                    inventoryToToggle.transform.localScale = new Vector3(0, 0, 0);
                    tempScript.setPauseActive(2);
                }
            }


        }
        if (Input.GetKeyDown(equipmentKeyCode))
        {
            //equipmentToToggle.SetActive(!equipmentToToggle.activeSelf);
            if (equipmentToToggle.transform.localScale == new Vector3(0, 0, 0))
            {
                if(tempScript.menuPause || tempScript.chatPause);
                else{
                    tempScript.setPauseActive(3);
                    equipmentToToggle.transform.localScale = new Vector3(1, 1, 1);
                }
            }
            else
            {
                if(tempScript.chatPause);
                else{
                    tempScript.setPauseActive(3);
                    equipmentToToggle.transform.localScale = new Vector3(0, 0, 0);
                }
            }
        }


        if (Input.GetKeyDown(firstHotBarSlotKeyCode))
        {
            hotbarSlots[0].GetComponent<HotbarSlot>().UseSlot(0);

        }
        if (Input.GetKeyDown(secondHotBarSlotKeyCode))
        {
            hotbarSlots[1].GetComponent<HotbarSlot>().UseSlot(1);
        }
        if (Input.GetKeyDown(thirdHotBarSlotKeyCode))
        {
            hotbarSlots[2].GetComponent<HotbarSlot>().UseSlot(2);
        }


        /*
        if (inventoryToToggle.activeSelf || equipmentToToggle.activeSelf)
        {
            ShowMouseCursor();
        }
        else
        {
            HideMouseCursor();
        } */
        }
    }

    public void ShowMouseCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideMouseCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
