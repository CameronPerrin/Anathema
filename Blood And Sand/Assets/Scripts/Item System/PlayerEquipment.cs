using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public enum EquipSlot
    {
        Weapon, Head, Chest, Legs
    }

    private HotbarItem weaponItem;
    private HotbarItem headItem;
    private HotbarItem chestItem;
    private HotbarItem legsItem;

    public HotbarItem GetWeaponItem()
    {
        return weaponItem;
    }
    public HotbarItem GetHelmetItem()
    {
        return headItem;
    }
    public HotbarItem GetChestItem()
    {
        return chestItem;
    }
    public HotbarItem GetLegsItem()
    {
        return legsItem;
    }

    private void SetWeaponItem(HotbarItem weaponItem)
    {
        this.weaponItem = weaponItem;
        Debug.Log("Weapon Slot: Currently Equiped " + weaponItem.name);
    }
    private void SetHeadItem(HotbarItem headItem)
    {
        this.headItem = headItem;
    }
    private void SetChestItem(HotbarItem chestItem)
    {
        this.chestItem = chestItem;
    }

    private void SetLegsItem(HotbarItem legsItem)
    {
        this.legsItem = legsItem;
    }


    public void TryEquipItem(EquipSlot equipSlot, HotbarItem item)
    {
        if (equipSlot == item.GetEquipSlot())
        {
            // Item matches this EquipSlot
            switch (equipSlot)
            {
                default:
                case EquipSlot.Chest: SetChestItem(item); Debug.Log("Equiping Chest!!"); break;
                case EquipSlot.Head: SetHeadItem(item); Debug.Log("Equiping Head!!"); break;
                case EquipSlot.Legs: SetLegsItem(item); Debug.Log("Equiping Legs!!"); break;
                case EquipSlot.Weapon: SetWeaponItem(item); Debug.Log("Equiping Weapon!!"); break;
            }
        }
    }


}
