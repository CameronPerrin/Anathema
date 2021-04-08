using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public InventoryItem[] Items;
    public Dictionary<InventoryItem, int> GetId = new Dictionary<InventoryItem, int>();
    public Dictionary<int, InventoryItem> GetItem = new Dictionary<int, InventoryItem>();


    public void OnAfterDeserialize()
    {
        GetId = new Dictionary<InventoryItem, int>();
        GetItem = new Dictionary<int, InventoryItem>();
        for(int i = 0; i < Items.Length; i++)
        {
            GetId.Add(Items[i], i);
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
    }
}
    
