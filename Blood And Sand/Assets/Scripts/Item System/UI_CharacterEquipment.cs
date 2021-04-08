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
    
    public void loadEquippedItems()
    {
        weaponSlot = transform.Find("weaponSlot").GetComponent<InventorySlot>();
        //headSlot = transform.Find("headSlot").GetComponent<InventorySlot>();
        //chestSlot = transform.Find("chestSlot").GetComponent<InventorySlot>();
        //legSlot = transform.Find("legSlot").GetComponent<InventorySlot>();
        
        //playerEquipment.TryEquipItem(PlayerEquipment.EquipSlot.Weapon, weaponSlot.SlotItem, true);
        StartCoroutine(waitForSlotLoad(weaponSlot));


    }

    private void WeaponSlot_OnItemDropped(object sender, InventorySlot.OnItemDroppedEventArgs e)
    {
        // Item dropped in weapon slot
        //playerEquipment.TryEquipItem(PlayerEquipment.EquipSlot.Weapon, e.item, false);
        StartCoroutine(waitForSlot(e));
    }
    private void HeadSlot_OnItemDropped(object sender, InventorySlot.OnItemDroppedEventArgs e)
    {
        // Item dropped in weapon slot
        playerEquipment.TryEquipItem(PlayerEquipment.EquipSlot.Head, e.item, false);
    }
    private void ChestSlot_OnItemDropped(object sender, InventorySlot.OnItemDroppedEventArgs e)
    {
        // Item dropped in weapon slot
        playerEquipment.TryEquipItem(PlayerEquipment.EquipSlot.Chest, e.item, false);
    }
    private void LegSlot_OnItemDropped(object sender, InventorySlot.OnItemDroppedEventArgs e)
    {
        // Item dropped in weapon slot
        playerEquipment.TryEquipItem(PlayerEquipment.EquipSlot.Legs, e.item, false);
    }


    IEnumerator waitForSlot(InventorySlot.OnItemDroppedEventArgs inventorySlot)
    {
        yield return new WaitForSeconds(0.1f);
        playerEquipment.TryEquipItem(PlayerEquipment.EquipSlot.Weapon, inventorySlot.item, false);
    }

    IEnumerator waitForSlotLoad(InventorySlot inventorySlot)
    {
        yield return new WaitForSeconds(0.1f);
        playerEquipment.TryEquipItem(PlayerEquipment.EquipSlot.Weapon, inventorySlot.SlotItem, false);
    }

    public void SetCharacterEquipment(PlayerEquipment playerEquipment)
    {
        this.playerEquipment = playerEquipment;
    }
}
