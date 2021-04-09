using UnityEngine;

[CreateAssetMenu]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] InventoryItem[] items;

    public InventoryItem GetItemReference(string itemID)
    {
        foreach (InventoryItem item in items)
        {
            if (item.ID == itemID)
            {
                return item;
            }
        }
        return null;
    }

    public InventoryItem GetItemCopy(string itemID)
    {
        InventoryItem item = GetItemReference(itemID);
        if (item == null)
        {
            Debug.Log("Item not found"); 
            return null;
        }


        return item.GetCopy();
    }

}