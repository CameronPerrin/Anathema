using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;


[CreateAssetMenu(fileName = "New Equipment", menuName = "Items/Equipment")]
public class EquipmentItem : InventoryItem
{


    [Header("Equipment Data")]
    [SerializeField] private EquipmentSlot equipSlot;

    [SerializeField] public GameObject weaponPrefab;

    [SerializeField] public Item_Data itemData;

    public override string GetInfoDisplayText()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(Rarity.Name).AppendLine();
        builder.Append(Description).AppendLine();

        if (itemData.attack != 0)
        {
            builder.Append("Damage: " + itemData.attack).AppendLine();
        }

        if(itemData.attack_speed != 0)
        {
            builder.Append("Attack Speed: " + itemData.attack_speed).AppendLine();
        }

        if(itemData.crit_chance != 0)
        {
            builder.Append("Crit Chance: " + itemData.crit_chance).AppendLine();
        }

        if(itemData.bleed_chance != 0)
        {
            builder.Append("Bleed Chance: " + itemData.bleed_chance).AppendLine();
        }

        if (itemData.range != 0)
        {
            builder.Append("Range: " + itemData.range).AppendLine();
        }

        if(itemData.defense != 0)
        {
            builder.Append("Defense: " + itemData.defense).AppendLine();
        }
        
        if(itemData.magic_defense != 0)
        {
            builder.Append("Magic Defense: " + itemData.magic_defense).AppendLine();
        }

        if(itemData.move_speed != 0)
        {
            builder.Append("Move Speed: " + itemData.move_speed).AppendLine();
        }

        //builder.Append("Sell Price: ").Append(SellPrice).Append(" Essence");
        return builder.ToString();
    }







}


