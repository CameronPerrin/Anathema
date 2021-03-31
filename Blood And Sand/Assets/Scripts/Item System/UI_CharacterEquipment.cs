using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterEquipment : MonoBehaviour
{
    private InventorySlot weaponSlot;
    private InventorySlot headSlot;
    private InventorySlot chestSlot;
    private InventorySlot legSlot;
    private PlayerEquipment playerEquipment;

    private void Awake()
    {
        weaponSlot = transform.Find("weaponSlot").GetComponent<InventorySlot>();
        headSlot = transform.Find("headSlot").GetComponent<InventorySlot>();
        chestSlot = transform.Find("chestSlot").GetComponent<InventorySlot>();
        legSlot = transform.Find("legSlot").GetComponent<InventorySlot>();


        weaponSlot.OnItemDropped += WeaponSlot_OnItemDropped;
        headSlot.OnItemDropped += HeadSlot_OnItemDropped;
        chestSlot.OnItemDropped += ChestSlot_OnItemDropped;
        legSlot.OnItemDropped += LegSlot_OnItemDropped;
    }

    private void WeaponSlot_OnItemDropped(object sender, InventorySlot.OnItemDroppedEventArgs e)
    {
        // Item dropped in weapon slot
        playerEquipment.TryEquipItem(PlayerEquipment.EquipSlot.Weapon, e.item);
    }
    private void HeadSlot_OnItemDropped(object sender, InventorySlot.OnItemDroppedEventArgs e)
    {
        // Item dropped in weapon slot
        playerEquipment.TryEquipItem(PlayerEquipment.EquipSlot.Head, e.item);
    }
    private void ChestSlot_OnItemDropped(object sender, InventorySlot.OnItemDroppedEventArgs e)
    {
        // Item dropped in weapon slot
        playerEquipment.TryEquipItem(PlayerEquipment.EquipSlot.Chest, e.item);
    }
    private void LegSlot_OnItemDropped(object sender, InventorySlot.OnItemDroppedEventArgs e)
    {
        // Item dropped in weapon slot
        playerEquipment.TryEquipItem(PlayerEquipment.EquipSlot.Legs, e.item);
    }

    public void SetCharacterEquipment(PlayerEquipment playerEquipment)
    {
        this.playerEquipment = playerEquipment;
    }
}
