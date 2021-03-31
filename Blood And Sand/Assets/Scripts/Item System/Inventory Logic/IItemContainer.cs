using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tells what the item container needs to have but not how it needs to do it.
public interface IItemContainer
{
    ItemSlot AddItem(ItemSlot itemSlot);
    void RemoveItem(ItemSlot itemSlot);
    void RemoveAt(int slotIndex);
    void Swap(int indexOne, int indexTwo);
    public bool HasItem(InventoryItem item);
    int GetTotalQuantity(InventoryItem item);
    void UseItem(InventoryItem item);
}
