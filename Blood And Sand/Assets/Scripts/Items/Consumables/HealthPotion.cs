using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Items/Consumable Item/Health Potion")]
public class HealthPotion : ConsumableItem
{
    public override void Use()
    {
        Debug.Log("Drinking Health Potion!!");
    }
}
