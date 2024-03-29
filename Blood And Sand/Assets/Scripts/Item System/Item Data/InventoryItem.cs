using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem : HotbarItem
{
    [Header("Item Data")]
    [SerializeField] private Rarity rarity = null;
    [SerializeField] [Min(0)] private int sellPrice = 1;
    [SerializeField] [Min(1)] private int maxStack = 1;

    public override string ColouredName
    {
        get 
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(rarity.TextColour);
            return $"<color=#{hexColor}>{Name}</color>";
        }
    }

    public InventoryItem GetCopy()
    {
        return this;
    }

    public int SellPrice => sellPrice;
    public int MaxStack => maxStack;
    public Rarity Rarity => rarity;


}
