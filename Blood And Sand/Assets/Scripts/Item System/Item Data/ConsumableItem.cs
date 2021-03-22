using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Items/Consumable Item")]
public class ConsumableItem : InventoryItem
{
    [Header("Consumable Data")]
    [SerializeField] private string useText = "Does Something, maybe?";

    public override string GetInfoDisplayText()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(Rarity.Name).AppendLine();
        builder.Append(Description).AppendLine();
        builder.Append("<color=green>Use: ").Append(useText).Append("</color>").AppendLine();
        builder.Append("Max stack: ").Append(MaxStack).AppendLine();
        builder.Append("Sell Price: ").Append(SellPrice).Append(" Essence");

        return builder.ToString();
    }
}
