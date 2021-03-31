using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentBar : MonoBehaviour
{
    [SerializeField] private EquipmentSlot[] equipmentSlots = new EquipmentSlot[5];

    public void Add(EquipmentItem equipmentItem)
    {
        foreach(EquipmentSlot equipSlot in equipmentSlots)
        {
            if(equipSlot.AddItem(equipmentItem)) { return; }
        } 

    }


}
