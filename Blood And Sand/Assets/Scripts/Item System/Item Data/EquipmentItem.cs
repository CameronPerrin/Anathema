using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


[CreateAssetMenu(fileName = "New Equipment", menuName = "Items/Equipment")]
public class EquipmentItem : InventoryItem
{


    [Header("Equipment Data")]
    [SerializeField] private EquipmentSlot equipSlot;
    [SerializeField] private int armorModifier;
    [SerializeField] private int damageModifier;

    public override string GetInfoDisplayText()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(Rarity.Name).AppendLine();
        builder.Append(Description).AppendLine();
        builder.Append("Max stack: ").Append(MaxStack).AppendLine();
        builder.Append("Sell Price: ").Append(SellPrice).Append(" Essence");
        return builder.ToString();
    }





}


