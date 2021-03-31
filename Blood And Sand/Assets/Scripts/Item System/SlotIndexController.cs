using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotIndexController : MonoBehaviour
{
    public List<GameObject> SlotIndexes;
    public List<GameObject> HotBarSlotIndexes;

    private void Awake()
    {
        Transform[] allchildren = this.transform.GetComponentsInChildren<Transform>(true);
        for(int i = 0; i < allchildren.Length; i++)
        {
            if(allchildren[i].tag == "Inventory_Slot")
            {
                SlotIndexes.Add(allchildren[i].gameObject);
            }
            else if (allchildren[i].tag == "Hotbar_Slot")
            {
                HotBarSlotIndexes.Add(allchildren[i].gameObject);
            }
        }
    }
}
