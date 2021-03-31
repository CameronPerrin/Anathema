using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : ItemSlotUI, IDropHandler
{
    [SerializeField] private Inventory inventory = null;

    private HotbarItem slotItem = null;
    public override HotbarItem SlotItem 
    {
        get { return slotItem; }
        set { slotItem = value; UpdateSlotUI(); }
    } 

    public ItemSlot ItemSlot => inventory.ItemContainer.GetSlotByIndex(SlotIndex);
    public bool AddItem(EquipmentItem itemToAdd)
    {
        if(SlotItem != null) { return false; }
        SlotItem = itemToAdd;
        return true;
    } 

    public override void OnDrop(PointerEventData eventData)
    {
        ItemDragHandler itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
        if(itemDragHandler == null) { return;  Debug.Log("No inventory detected"); }

        InventorySlot inventorySlot = itemDragHandler.ItemSlotUI as InventorySlot;
        if (inventorySlot != null && inventorySlot.SlotItem.IsEquipment == true)
        {
            SlotItem = inventorySlot.ItemSlot.item;
            return;
        }
        EquipmentSlot equipmentSlot = itemDragHandler.ItemSlotUI as EquipmentSlot;
        if ((itemDragHandler.ItemSlotUI as EquipmentSlot) != null)
        {
            Debug.Log("first Swapping Initiated");
            inventory.ItemContainer.Swap(itemDragHandler.ItemSlotUI.SlotIndex, SlotIndex);
        }
        if (equipmentSlot == null)
        {
            Debug.Log("second Swapping Initiated");
            inventory.ItemContainer.Swap(itemDragHandler.ItemSlotUI.SlotIndex, SlotIndex);
            return;
        }
        /*
        EquipmentSlot equipmentSlot = itemDragHandler.ItemSlotUI as EquipmentSlot;
        if (inventorySlot != null)
        {
            // Swapping
            return;
        } */
    }

    public override void UpdateSlotUI()
    {
        if(slotItem == null)
        {
            EnableSlotUI(false);
            return;
        }

        itemIconImage.sprite = SlotItem.Icon;
        EnableSlotUI(true);
    }

    protected override void EnableSlotUI(bool enable)
    {
        base.EnableSlotUI(enable);
    }



}
