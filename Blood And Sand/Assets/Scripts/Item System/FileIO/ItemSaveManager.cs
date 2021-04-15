using System.Collections.Generic;
using UnityEngine;
using System;


public class ItemSaveManager : MonoBehaviour
{
    [SerializeField] ItemDatabase itemDatabase;
    [SerializeField] private UI_CharacterEquipment uiCharacterEquipment;
    [SerializeField] private Inventory playerInventory;

    private const string InventoryFileName = "Inventory";
    // The weapon the player has.



    private void Start()
    {
        try
        {
            LoadInventory(playerInventory);
        }
        catch (Exception e) {
            Debug.Log("There is no save file to load.");
        }
    }

    private void OnDestroy()
    {
        SaveInventory(playerInventory, playerInventory.Money);
    }


    public void LoadInventory(Inventory inventory)
    {
        //Debug.Log("Loading Inventory...");
        ItemContainerSaveData savedSlots = ItemSaveIO.LoadItems(InventoryFileName);
        inventory.Money = savedSlots.Money;
        if (savedSlots == null) return;

        inventory.Clear(); //Clear inventory

        for(int i = 0; i < savedSlots.SavedSlots.Length; i++)
        {
            //ItemSlot itemSlot = inventory.ItemContainer.itemSlots[i];
            ItemSlot itemSlot;
            ItemSlotSaveData savedSlot = savedSlots.SavedSlots[i];

            if(savedSlot == null)
            {
                itemSlot.item = null;
                itemSlot.quantity = 0;
            }
            else
            {

                itemSlot.item = itemDatabase.GetItemCopy(savedSlot.ItemID);
                itemSlot.quantity = savedSlot.Amount;
                inventory.ItemContainer.AddItemAt(itemSlot, i);

                if(itemSlot.item as EquipmentItem)
                {
                    (itemSlot.item as EquipmentItem).itemData = savedSlot.itemData;
                }
            }
        }

        if (savedSlots.SavedSlots[17] != null)
        {
            //Debug.Log("Loading Equipped Weapon from Save File...");
            uiCharacterEquipment.loadEquippedItems();
        }
    }

    public void SaveInventory(Inventory inventory, int currencyAmount)
    {
        //Debug.Log("Saving Inventory...");
        SaveItems(inventory.ItemContainer.itemSlots, InventoryFileName, currencyAmount);
    }

    private void SaveItems(IList<ItemSlot> itemSlots, string fileName, int currencyAmount)
    {
        var saveData = new ItemContainerSaveData(itemSlots.Count, currencyAmount);
        
        for(int i = 0; i < saveData.SavedSlots.Length; i++)
        {
            
            ItemSlot itemSlot = itemSlots[i];

            if(itemSlot.item == null)
            {
                saveData.SavedSlots[i] = null;
            }
            else
            {
                //Debug.Log("Saving " + itemSlot.item.ID + " to slot " + i);
                saveData.SavedSlots[i] = new ItemSlotSaveData(itemSlot.item.ID, itemSlot.quantity);

                if(itemSlot.item as EquipmentItem)
                {
                    //Debug.Log("Saving Equipment Item...");
                    saveData.SavedSlots[i].itemData = (itemSlot.item as EquipmentItem).itemData;
                }

            }
        }

        ItemSaveIO.SaveItems(saveData, fileName);
    }
    
}
