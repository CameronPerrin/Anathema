using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Photon.Pun;

public class InventoryManagerController : MonoBehaviour
{
    public GameObject inventoryPanel;
	public GameObject equipmentPanel;

	public Inventory playerInventory;

	[SerializeField] ItemSaveManager itemSaveManager;



    private void Start()
    {
		itemSaveManager.LoadInventory(playerInventory);
	}
    private void Update()
    {
		if(Input.GetKeyDown(KeyCode.P))
        {
			itemSaveManager.LoadInventory(playerInventory);
		}
		if (Input.GetKeyDown(KeyCode.O))
		{
			itemSaveManager.SaveInventory(playerInventory, playerInventory.Money);
		}

		if (inventoryPanel.transform.localScale == new Vector3(0,0,0) || equipmentPanel.transform.localScale == new Vector3(0, 0, 0))
        {
			itemSaveManager.SaveInventory(playerInventory, playerInventory.Money);
		}
	}
}