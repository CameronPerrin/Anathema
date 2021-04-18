using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveWithKeyPress : MonoBehaviour
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


    private void Start()
    {
        hotbarSlots = GameObject.FindGameObjectsWithTag("Hotbar_Slot");
        playerMouseLock = GameObject.Find("PhotonPlayer(Clone)");

        inventoryToToggle.transform.localScale = new Vector3(0, 0, 0);
        equipmentToToggle.transform.localScale = new Vector3(0, 0, 0);
    }

    private void Update()
    {

        if (Input.GetKeyDown(inventoryKeyCode) && !isPaused)
        {
            //inventoryToToggle.SetActive(!inventoryToToggle.activeSelf);

            if (inventoryToToggle.transform.localScale == new Vector3(0, 0, 0))
            {
                inventoryToToggle.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                inventoryToToggle.transform.localScale = new Vector3(0, 0, 0);
            }


        }
        if (Input.GetKeyDown(equipmentKeyCode) && !isPaused)
        {
            //equipmentToToggle.SetActive(!equipmentToToggle.activeSelf);
            if (equipmentToToggle.transform.localScale == new Vector3(0, 0, 0))
            {
                equipmentToToggle.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                equipmentToToggle.transform.localScale = new Vector3(0, 0, 0);
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
