using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarSlot : ItemSlotUI, IDropHandler
{
    [SerializeField] private Inventory inventory = null;
    [SerializeField] private TextMeshProUGUI itemQuantityText = null;

    private HotbarItem slotItem = null;
    public override HotbarItem SlotItem
    {
        get { return slotItem; }
        set { slotItem = value; UpdateSlotUI(); }
    }

    public bool AddItem(HotbarItem itemToAdd)
    {
        if(slotItem != null) { return false; }
        SlotItem = itemToAdd;
        return true;
    }

    public void UseSlot(int index)
    {
        Debug.Log(SlotIndex);
        if (index != SlotIndex) { Debug.Log("Can't find inventory slot"); return; }
        //Use Item
        // When key is pressed, loop through inventory slots
        if (slotItem is InventoryItem inventoryItem && slotItem is ConsumableItem consumableItem)
        {
            if (inventory.ItemContainer.HasItem(inventoryItem))
            {
                inventory.ItemContainer.UseItem(inventoryItem);
                consumableItem.Use();
            }
            else
                SlotItem = null;
        }


    }

    public override void OnDrop(PointerEventData eventData)
    {
        ItemDragHandler itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
        if(itemDragHandler == null) { return; }

        InventorySlot inventorySlot = itemDragHandler.ItemSlotUI as InventorySlot;
        if(inventorySlot != null)
        {
            SlotItem = inventorySlot.ItemSlot.item;
            return;
        }
        HotbarSlot hotbarSlot = itemDragHandler.ItemSlotUI as HotbarSlot;
        if(hotbarSlot != null)
        {
            HotbarItem oldItem = SlotItem;
            SlotItem = hotbarSlot.SlotItem;
            hotbarSlot.SlotItem = oldItem;
            return;
        }

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

        SetItemQuantityUI();
    }

    private void SetItemQuantityUI()
    {
        if(slotItem is InventoryItem inventoryItem)
        {
            if (inventory.ItemContainer.HasItem(inventoryItem))
            {
                int quantityCount = inventory.ItemContainer.GetTotalQuantity(inventoryItem);
                itemQuantityText.text = quantityCount > 1 ? quantityCount.ToString() : "";
            }
            else
                SlotItem = null;
        }
        else
        {
            itemQuantityText.enabled = false;
        }
    }

    protected override void EnableSlotUI(bool enable)
    {
        base.EnableSlotUI(enable);
        itemQuantityText.enabled = enable;
    }
}
