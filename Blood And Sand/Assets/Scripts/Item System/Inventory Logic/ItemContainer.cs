using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
[Serializable]
public class ItemContainer : IItemContainer
{
    public ItemSlot[] itemSlots = new ItemSlot[0];

    public Action OnItemsUpdated = delegate { }; // Update UI

    public ItemContainer(int size) => itemSlots = new ItemSlot[size];

    public ItemSlot GetSlotByIndex(int index) => itemSlots[index];

    public ItemSlot AddItem(ItemSlot itemSlot)
    {
        // Deals with adding to existing item in the inventory
        // Loop through item slots
        for(int i = 0; i < itemSlots.Length - 4; i++)
        {
            // If item slot has an item
            if(itemSlots[i].item != null)
            {
                // If item slot is the same item we are trying to add
                if(itemSlots[i].item == itemSlot.item)
                {
                    int slotRemainingSpace = itemSlots[i].item.MaxStack - itemSlots[i].quantity;

                    if(itemSlot.quantity <= slotRemainingSpace)
                    {
                        itemSlots[i].quantity += itemSlot.quantity;
                        itemSlot.quantity = 0; // no more quantity to add

                        OnItemsUpdated.Invoke();

                        return itemSlot;
                    }
                    else if(slotRemainingSpace > 0 ) // Add some but not all of the quantity we want to add
                    {
                        itemSlots[i].quantity += slotRemainingSpace;
                        itemSlot.quantity -= slotRemainingSpace;
                    }
                }
            }

        }

        // Deals with adding new items or empty inventory
        for (int i = 0; i < itemSlots.Length - 4; i++)
        {
            if(itemSlots[i].item == null)
            {
                if(itemSlot.quantity <= itemSlot.item.MaxStack)
                {
                    itemSlots[i] = itemSlot;
                    itemSlot.quantity = 0; // no more quantity to add

                    OnItemsUpdated.Invoke();

                    return itemSlot;
                }
                else
                {
                    itemSlots[i] = new ItemSlot(itemSlot.item, itemSlot.item.MaxStack);
                    itemSlot.quantity -= itemSlot.item.MaxStack; // no more quantity to add
                }
            }
        }

        OnItemsUpdated.Invoke();

        return itemSlot;
    }

    public void AddItemAt(ItemSlot itemSlot, int slotIndex)
    {
        // Make sure the index slot is valid (between 0 and max item slots)
        if (slotIndex < 0 || slotIndex > itemSlots.Length - 1) { return; }

        itemSlots[slotIndex] = itemSlot;

        OnItemsUpdated.Invoke();
    }


    // Gets total amount of an item
    public int GetTotalQuantity(InventoryItem item)
    {
        int totalCount = 0;

        foreach (ItemSlot itemSlot in itemSlots)
        {
            // Dont add if slot is empty
            if (itemSlot.item == null) { continue; }
            // Don't add if slot is not item we want
            if (itemSlot.item != item) { continue; }
            totalCount += itemSlot.quantity;
        }

        return totalCount;
    }

    // Checks if an inventory has an item
    public bool HasItem(InventoryItem item)
    {
        // For slots of 16
        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.item == null) { continue; }
            if (itemSlot.item != item) { continue; }
            return true;
        }
        return false;

    }

    public void UseItem(InventoryItem item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            // Checks if item slot is not empty
            if (itemSlots[i].item != null)
            {
                // Check if the item is the item we want to remove
                if (itemSlots[i].item == item)
                {
                    Debug.Log("Item Quantity Before: " + itemSlots[i].quantity);
                    itemSlots[i].quantity--;
                    Debug.Log("Item Quantity After: " + itemSlots[i].quantity);
                    if(itemSlots[i].quantity == 0)
                    {
                        itemSlots[i] = new ItemSlot();
                    }
                    OnItemsUpdated.Invoke();
                    return;
                }
            }
        }
    }

    // Removes item in item slot entirely
    public void RemoveAt(int slotIndex)
    {
        // Make sure the index slot is valid (between 0 and max item slots)
        if (slotIndex < 0 || slotIndex > itemSlots.Length - 1) { return; }
        itemSlots[slotIndex] = new ItemSlot();

        OnItemsUpdated.Invoke();
    }

    // Remove item in item slot by quantity
    public void RemoveItem(ItemSlot itemSlot)
    {
        // Looping through slots
        for (int i = 0; i < itemSlots.Length; i++)
        {
            // Checks if item slot is not empty
            if(itemSlots[i].item != null)
            {
                // Check if the item is the item we want to remove
                if(itemSlots[i].item == itemSlot.item)
                {
                    if(itemSlots[i].quantity < itemSlot.quantity)
                    {
                        itemSlot.quantity -= itemSlots[i].quantity;
                        itemSlots[i] = new ItemSlot();
                    }
                    else
                    {
                        itemSlots[i].quantity -= itemSlot.quantity;
                        if(itemSlots[i].quantity == 0)
                        {
                            itemSlots[i] = new ItemSlot();

                            OnItemsUpdated.Invoke();

                            return;
                        }
                    }
                }
            }
        }
    }

    public void Swap(int indexOne, int indexTwo)
    {
        ItemSlot firstSlot = itemSlots[indexOne];
        ItemSlot secondSlot = itemSlots[indexTwo];

        // See item slot operator in ItemSlot
        if (firstSlot == secondSlot) { return; }



        // If item is dropped onto an occupied slot
        if(secondSlot.item != null)
        {
            // Checks if both item slots are the same item
            if(firstSlot.item == secondSlot.item)
            {
                // Combine items
                int secondSlotRemainingSpace = secondSlot.item.MaxStack - secondSlot.quantity;
                if(firstSlot.quantity <= secondSlotRemainingSpace)
                {
                    // Shove all second slot into first slot
                    itemSlots[indexTwo].quantity += firstSlot.quantity;

                    // Set item slot empty;
                    itemSlots[indexOne] = new ItemSlot();

                    OnItemsUpdated.Invoke();

                    return;
                }
            }
        }
        // Else swap items
        itemSlots[indexOne] = secondSlot;
        itemSlots[indexTwo] = firstSlot;
        OnItemsUpdated.Invoke();
    }
    public void SwapEquip(int indexOne, int indexTwo)
    {
        ItemSlot firstSlot = itemSlots[indexOne];
        ItemSlot secondSlot = itemSlots[indexTwo];

        if(secondSlot.item == null)
        {
            itemSlots[indexOne] = secondSlot;
            itemSlots[indexTwo] = firstSlot;
            OnItemsUpdated.Invoke();
            return;
        }
        else if (secondSlot.item.IsEquipment)
        {
                if (firstSlot.item.equipmentType == secondSlot.item.equipmentType)
                {
                    Debug.Log("Second slot same equipment detected");
                    itemSlots[indexOne] = secondSlot;
                    itemSlots[indexTwo] = firstSlot;
                    OnItemsUpdated.Invoke();
                    return;
                }
                else
                {
                    return;
                }
        }
        else
        {
            return;
        }
        
        itemSlots[indexOne] = secondSlot;
        itemSlots[indexTwo] = firstSlot;
        OnItemsUpdated.Invoke(); 
    }

    public void ClearInventory()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i] = new ItemSlot();
        }
    }
}

