using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Struct uses values and not references
[Serializable]
public struct ItemSlot
{
    public InventoryItem item;
    [Min(1)] public int quantity;

    public ItemSlot(InventoryItem item, int quantity)
    {
      this.item = item;
      this.quantity = quantity;
    }

    public static bool operator == (ItemSlot a, ItemSlot b) { return a.Equals(b); }
    public static bool operator != (ItemSlot a, ItemSlot b) { return !a.Equals(b); }

}
