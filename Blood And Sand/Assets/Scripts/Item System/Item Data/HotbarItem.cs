using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base Class of Inventory

// Nothing will be of type HotbarItem
public abstract class HotbarItem : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] string id;
    public string ID { get { return name; } }
    [SerializeField] private new string name = "New HotBar Item Name";
    [SerializeField] private new string description = "New HotBar Item Description";
    [SerializeField] private Sprite icon = null;
    [SerializeField] protected bool isEquipment = false;
    [SerializeField] public EquipmentType equipmentType;
    public enum EquipmentType { None, Head, Chest, Legs, Weapon }

    // Other classes will not change name but can reference it (get)
    public string Name => name;
    public string Description => description;

    // Inherented class needs to modify ColouredName
    public abstract string ColouredName { get;  }

    public Sprite Icon => icon;
    public bool IsEquipment => isEquipment;

    // Inherented classes needs to modify GetInfoDisplayText to display different things
    public abstract string GetInfoDisplayText();

    public PlayerEquipment.EquipSlot GetEquipSlot()
    {
        if (IsEquipment == true)
        {
            switch (equipmentType)
            {
                default:
                case EquipmentType.Head:
                    return PlayerEquipment.EquipSlot.Head;
                case EquipmentType.Chest:
                    return PlayerEquipment.EquipSlot.Chest;
                case EquipmentType.Legs:
                    return PlayerEquipment.EquipSlot.Legs;
                case EquipmentType.Weapon:
                    return PlayerEquipment.EquipSlot.Weapon;
            }
        }
        else
            return 0;
    }


}

