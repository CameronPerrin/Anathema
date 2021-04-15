using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

    //The current user.
    public GameObject CurrentPlayer;
    private GameObject LoadedWeapon;
    private Item_Data LoadedWeaponItemData;
    private EquipmentItem equipmentItem;

    [SerializeField] private Inventory playerInventory;
    [SerializeField] ItemSaveManager itemSaveManager;

    PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (playerInventory.ItemContainer.itemSlots[17].item == null)
        {
            if(LoadedWeapon)
            {
                PhotonNetwork.Destroy(LoadedWeapon);
            }
        }


        if(playerInventory.ItemContainer.itemSlots[17].item)
        {
            if(LoadedWeapon)
            {
                LoadedWeaponItemData = LoadedWeapon.GetComponent<WeaponStats>().returnItemData();
                bool isWeaponSame = (playerInventory.ItemContainer.itemSlots[17].item as EquipmentItem).itemData.Equals(LoadedWeaponItemData);
                if (!isWeaponSame)
                {
                    Debug.Log((playerInventory.ItemContainer.itemSlots[17].item as EquipmentItem).itemData.attack);
                    Debug.Log(LoadedWeaponItemData.attack);
                    SetWeaponItem(playerInventory.ItemContainer.itemSlots[17].item, false);
                    //
                } 
            }

        }




    }
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
    private void SetWeaponItem(HotbarItem weaponItem, bool isLoadingfromSave)
    {
        this.weaponItem = weaponItem;
        Debug.Log("Weapon Slot: Currently Equiped " + weaponItem.name);

        if (LoadedWeapon)
        {
            PhotonNetwork.Destroy(LoadedWeapon);
        }

        if (isLoadingfromSave)
        {
            ItemContainerSaveData savedSlots = ItemSaveIO.LoadItems("Inventory");
            if (savedSlots.SavedSlots[17] != null)
            {
                CurrentPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
                LoadedWeapon = PhotonNetwork.Instantiate((Path.Combine("PhotonPrefabs/Items", savedSlots.SavedSlots[17].ItemID)), Vector3.up, Quaternion.identity) as GameObject;
                LoadedWeapon.transform.position = CurrentPlayer.transform.GetChild(1).GetChild(0).position;
                LoadedWeapon.transform.rotation = CurrentPlayer.transform.GetChild(1).GetChild(0).rotation;

                PV.RPC("TransformLoadedWeapon", RpcTarget.All);
                //LoadedWeapon.transform.SetParent(CurrentPlayer.transform.GetChild(1).GetChild(0));
                //LoadedWeapon.transform.parent = CurrentPlayer.transform.GetChild(1).GetChild(0);

                LoadedWeapon.GetComponent<WeaponStats>().item_value = savedSlots.SavedSlots[17].itemData.item_value;
                LoadedWeapon.GetComponent<WeaponStats>().attack = savedSlots.SavedSlots[17].itemData.attack;
                LoadedWeapon.GetComponent<WeaponStats>().attack_speed = savedSlots.SavedSlots[17].itemData.attack_speed;
                LoadedWeapon.GetComponent<WeaponStats>().crit_chance = savedSlots.SavedSlots[17].itemData.crit_chance;
                LoadedWeapon.GetComponent<WeaponStats>().bleed_chance = savedSlots.SavedSlots[17].itemData.bleed_chance;
                LoadedWeapon.GetComponent<WeaponStats>().range = savedSlots.SavedSlots[17].itemData.range;
                LoadedWeapon.GetComponent<WeaponStats>().defense = savedSlots.SavedSlots[17].itemData.defense;
                LoadedWeapon.GetComponent<WeaponStats>().magic_defense = savedSlots.SavedSlots[17].itemData.magic_defense;
                LoadedWeapon.GetComponent<WeaponStats>().move_speed = savedSlots.SavedSlots[17].itemData.move_speed;
                LoadedWeapon.GetComponent<WeaponStats>().item_type = savedSlots.SavedSlots[17].itemData.item_type;
            }
        }
        else
        {
            CurrentPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
            LoadedWeapon = PhotonNetwork.Instantiate((Path.Combine("PhotonPrefabs/Items", weaponItem.Name)), Vector3.up, Quaternion.identity) as GameObject;
            LoadedWeapon.transform.position = CurrentPlayer.transform.GetChild(1).GetChild(0).position;
            LoadedWeapon.transform.rotation = CurrentPlayer.transform.GetChild(1).GetChild(0).rotation;

            PV.RPC("TransformLoadedWeapon", RpcTarget.All);
            //LoadedWeapon.transform.SetParent(CurrentPlayer.transform.GetChild(1).GetChild(0));
            //LoadedWeapon.transform.parent = CurrentPlayer.transform.GetChild(1).GetChild(0);

            equipmentItem = playerInventory.ItemContainer.itemSlots[17].item as EquipmentItem;

            LoadedWeapon.GetComponent<WeaponStats>().item_value = (playerInventory.ItemContainer.itemSlots[17].item as EquipmentItem).itemData.item_value;
            LoadedWeapon.GetComponent<WeaponStats>().attack = (playerInventory.ItemContainer.itemSlots[17].item as EquipmentItem).itemData.attack;
            LoadedWeapon.GetComponent<WeaponStats>().attack_speed = (playerInventory.ItemContainer.itemSlots[17].item as EquipmentItem).itemData.attack_speed;
            LoadedWeapon.GetComponent<WeaponStats>().crit_chance = (playerInventory.ItemContainer.itemSlots[17].item as EquipmentItem).itemData.crit_chance;
            LoadedWeapon.GetComponent<WeaponStats>().bleed_chance = (playerInventory.ItemContainer.itemSlots[17].item as EquipmentItem).itemData.bleed_chance;
            LoadedWeapon.GetComponent<WeaponStats>().range = (playerInventory.ItemContainer.itemSlots[17].item as EquipmentItem).itemData.range;
            LoadedWeapon.GetComponent<WeaponStats>().defense = (playerInventory.ItemContainer.itemSlots[17].item as EquipmentItem).itemData.defense;
            LoadedWeapon.GetComponent<WeaponStats>().magic_defense = (playerInventory.ItemContainer.itemSlots[17].item as EquipmentItem).itemData.magic_defense;
            LoadedWeapon.GetComponent<WeaponStats>().move_speed = (playerInventory.ItemContainer.itemSlots[17].item as EquipmentItem).itemData.move_speed;
            LoadedWeapon.GetComponent<WeaponStats>().item_type = (playerInventory.ItemContainer.itemSlots[17].item as EquipmentItem).itemData.item_type;
        }

        //LoadIntoHand(weaponItem, isLoadingfromSave);
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

    public void TryEquipItem(EquipSlot equipSlot, HotbarItem item, bool isLoadingfromSave)
    {
        if (equipSlot == item.GetEquipSlot())
        {
            // Item matches this EquipSlot
            switch (equipSlot)
            {
                default:
                case EquipSlot.Chest: SetChestItem(item); break;
                case EquipSlot.Head: SetHeadItem(item); break;
                case EquipSlot.Legs: SetLegsItem(item); break;
                case EquipSlot.Weapon: SetWeaponItem(item, isLoadingfromSave); break;
            }
        }
    }


    public void removeDisplayedEquipment()
    {
        PhotonNetwork.Destroy(LoadedWeapon);
    }



    /*
    public void LoadIntoHand(HotbarItem weaponItem, bool isLoadingfromSave)
    {
        if(isLoadingfromSave)
        {
            ItemContainerSaveData savedSlots = ItemSaveIO.LoadItems("Inventory");
            if (savedSlots.SavedSlots[17] != null)
            {
                CurrentPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
                LoadedWeapon = PhotonNetwork.Instantiate((Path.Combine("PhotonPrefabs/Items", savedSlots.SavedSlots[17].ItemID)), Vector3.up, Quaternion.identity) as GameObject;
                LoadedWeapon.transform.position = CurrentPlayer.transform.GetChild(1).GetChild(0).position;
                LoadedWeapon.transform.rotation = CurrentPlayer.transform.GetChild(1).GetChild(0).rotation;
                LoadedWeapon.transform.parent = CurrentPlayer.transform.GetChild(1).GetChild(0);

                LoadedWeapon.GetComponent<WeaponStats>().item_value = savedSlots.SavedSlots[17].itemData.item_value;
                LoadedWeapon.GetComponent<WeaponStats>().attack = savedSlots.SavedSlots[17].itemData.attack;
                LoadedWeapon.GetComponent<WeaponStats>().attack_speed = savedSlots.SavedSlots[17].itemData.attack_speed;
                LoadedWeapon.GetComponent<WeaponStats>().crit_chance = savedSlots.SavedSlots[17].itemData.crit_chance;
                LoadedWeapon.GetComponent<WeaponStats>().range = savedSlots.SavedSlots[17].itemData.range;
                LoadedWeapon.GetComponent<WeaponStats>().defense = savedSlots.SavedSlots[17].itemData.defense;
                LoadedWeapon.GetComponent<WeaponStats>().magic_defense = savedSlots.SavedSlots[17].itemData.magic_defense;
                LoadedWeapon.GetComponent<WeaponStats>().move_speed = savedSlots.SavedSlots[17].itemData.move_speed;
                LoadedWeapon.GetComponent<WeaponStats>().item_type = savedSlots.SavedSlots[17].itemData.item_type;
            }
        }
        else
        {
            LoadedWeapon = PhotonNetwork.Instantiate((Path.Combine("PhotonPrefabs/Items", weaponItem.name)), Vector3.up, Quaternion.identity) as GameObject;
            LoadedWeapon.transform.position = CurrentPlayer.transform.GetChild(1).GetChild(0).position;
            LoadedWeapon.transform.rotation = CurrentPlayer.transform.GetChild(1).GetChild(0).rotation;
            LoadedWeapon.transform.parent = CurrentPlayer.transform.GetChild(1).GetChild(0);
        }

        //if (savedSlots[])


     
    } */


    [PunRPC] void TransformLoadedWeapon()
    {
        Debug.Log("RPC Calling tranform");
        CurrentPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
        LoadedWeapon.transform.SetParent(CurrentPlayer.transform.GetChild(1).GetChild(0));
    }
}
