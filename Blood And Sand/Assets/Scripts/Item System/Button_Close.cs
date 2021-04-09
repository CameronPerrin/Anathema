using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Close : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject equipmentPanel;

    public void scaleInventory()
    {
        inventoryPanel.transform.localScale = new Vector3(0, 0, 0);
    }

    public void scaleEquipment()
    {
        equipmentPanel.transform.localScale = new Vector3(0, 0, 0);
    }
}
