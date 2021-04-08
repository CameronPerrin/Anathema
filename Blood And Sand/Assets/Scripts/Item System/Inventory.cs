using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Items/Inventory")]
public class Inventory : ScriptableObject
{
    public int Money = 1000;
    [SerializeField] private VoidEvent onInventoryItemsUpdated = null;
    [SerializeField] private ItemSlot testItemSlot = new ItemSlot();
    //public ItemContainer ItemContainer { get; set; } = new ItemContainer(20);
    public ItemContainer ItemContainer { get; } = new ItemContainer(20);

    public void OnEnable() => ItemContainer.OnItemsUpdated += onInventoryItemsUpdated.Raise;
    public void OnDisable() => ItemContainer.OnItemsUpdated -= onInventoryItemsUpdated.Raise;

    [ContextMenu("Test Add")]
    public void TestAdd()
    {
        ItemContainer.AddItem(testItemSlot);
        //ItemContainer.Save();
    }

    public void Clear()
    {
        ItemContainer.ClearInventory();
    }

}
