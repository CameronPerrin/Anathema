using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    EquipmentItem[] currentEquipment;
    private void Start()
    {
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new EquipmentItem[numSlots];
    }

    public void Equip (EquipmentItem newItem)
    {
        //int slotIndex = (int)newItem.equipSlot;
        //currentEquipment[slotIndex] = newItem;
    }
}
