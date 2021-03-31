using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : ItemSlotUI, IDropHandler
{
    [SerializeField] private Inventory inventory = null;
    [SerializeField] private TextMeshProUGUI itemQuantityText = null;
    [SerializeField] private InventorySlotType inventorySlotType;

    private HotbarItem slotItem = null;
    public override HotbarItem SlotItem
    {
        get { return ItemSlot.item;}
        set { slotItem = value; UpdateSlotUI(); }
    }

    public ItemSlot ItemSlot => inventory.ItemContainer.GetSlotByIndex(SlotIndex);
    public event EventHandler<OnItemDroppedEventArgs> OnItemDropped;
    public class OnItemDroppedEventArgs : EventArgs
    {
        public HotbarItem item;
    }
    public override void OnDrop(PointerEventData eventData)
    {
        ItemDragHandler itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
        if(itemDragHandler == null) { return; }

        InventorySlot inventorySlot = itemDragHandler.ItemSlotUI as InventorySlot;

        // If item is an equipment
        if(inventorySlot != null && inventorySlot.SlotItem.IsEquipment == true)
        {
            // Check if inventory slot is a chest slot and the item dropped is also a chest slot.
            if (inventorySlotType == InventorySlotType.Chest && inventorySlot.SlotItem.equipmentType == HotbarItem.EquipmentType.Chest)
            {
                Debug.Log("Testing chest slot");
                slotItem = inventorySlot.ItemSlot.item;
                OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { item = slotItem });
                inventory.ItemContainer.SwapEquip(itemDragHandler.ItemSlotUI.SlotIndex, SlotIndex);
                Debug.Log(slotItem.name);
                return;
            }
            // Check if inventory slot is a Head slot and the item dropped is also a Head slot.
            else if (inventorySlotType == InventorySlotType.Head && inventorySlot.SlotItem.equipmentType == HotbarItem.EquipmentType.Head)
            {
                slotItem = inventorySlot.ItemSlot.item;
                OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { item = slotItem });
                inventory.ItemContainer.SwapEquip(itemDragHandler.ItemSlotUI.SlotIndex, SlotIndex);
                Debug.Log(slotItem.name);
                return;
            }
            // Check if inventory slot is a legs slot and the item dropped is also a legs slot.
            else if (inventorySlotType == InventorySlotType.Legs && inventorySlot.SlotItem.equipmentType == HotbarItem.EquipmentType.Legs)
            {
                slotItem = inventorySlot.ItemSlot.item;
                OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { item = slotItem });
                inventory.ItemContainer.SwapEquip(itemDragHandler.ItemSlotUI.SlotIndex, SlotIndex);
                Debug.Log(slotItem.name);
                return;
            }
            // Check if inventory slot is a weapon slot and the item dropped is also a weapon slot.
            else if (inventorySlotType == InventorySlotType.Weapon && inventorySlot.SlotItem.equipmentType == HotbarItem.EquipmentType.Weapon)
            {
                slotItem = inventorySlot.ItemSlot.item;
                OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { item = slotItem });
                inventory.ItemContainer.SwapEquip(itemDragHandler.ItemSlotUI.SlotIndex, SlotIndex);
                Debug.Log(slotItem.name);
                return;
            }
            else if (inventorySlotType == InventorySlotType.Inventory)
            {
                Debug.Log("Inventory slot: inventory");
                inventory.ItemContainer.SwapEquip(itemDragHandler.ItemSlotUI.SlotIndex, SlotIndex); ;
                return;
            }
            else
            {
                return;
            }

            //return;
        }
        if ((itemDragHandler.ItemSlotUI as InventorySlot) != null & !CheckIfEquipmentSlot(inventorySlotType))
        {
            inventory.ItemContainer.Swap(itemDragHandler.ItemSlotUI.SlotIndex, SlotIndex);
            //return;
        }

        // If inventory slot && item is Equipment
            // if: Compare Head 
                // Do something
            // if: Compare Chest
                // Do Something
            // if: Compare Legs
                // Do something
            // if: Compare Weapon
                // Do Something
    }

    public override void UpdateSlotUI()
    {
        if(ItemSlot.item == null)
        {
            EnableSlotUI(false);
            return;
        }
        EnableSlotUI(true);
        itemIconImage.sprite = ItemSlot.item.Icon;
        itemQuantityText.text = ItemSlot.quantity > 1 ? ItemSlot.quantity.ToString() : "";
    }


    protected override void EnableSlotUI(bool enable)
    {
        base.EnableSlotUI(enable);
        itemQuantityText.enabled = enable;
    }

    public enum InventorySlotType { Inventory, Head, Chest, Legs, Weapon }

    private bool CheckIfEquipmentSlot(InventorySlotType inventorySlotType)
    {
        switch(inventorySlotType)
        {
            default: return true;
            case InventorySlotType.Inventory:
                return false;
        }
    }
}
