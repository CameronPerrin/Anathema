using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ItemSlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] public Image itemIconImage = null;
    public int SlotIndex { get; private set; }

    [SerializeField] private slotType slot;

    //public GameObject[] SlotIndexes;
    [SerializeField] private GameObject slotIndexController;

    [SerializeField] private GameObject thisSlot;
    public abstract HotbarItem SlotItem {get; set;}

    private void OnEnable() => UpdateSlotUI();

    /*
    protected virtual void Awake()
    {
        SlotIndexes = GameObject.FindGameObjectsWithTag("Inventory_Slot");
        SlotIndex = System.Array.IndexOf(SlotIndexes, thisSlot);
    } */
    protected virtual void Start()
    {
        //SlotIndexes = GameObject.FindGameObjectsWithTag("Inventory_Slot");
        //SlotIndex = System.Array.IndexOf(SlotIndexes, thisSlot);
        //SlotIndex = System.Array.IndexOf(slotIndexController.GetComponent<SlotIndexController>().SlotIndexes.ToArray(), thisSlot);
        //SlotIndex = slotIndexController.GetComponent<SlotIndexController>().SlotIndexes.IndexOf(thisSlot);
        //SlotIndex = transform.GetSiblingIndex();

        switch(slot)
        {
            case slotType.INVENTORY:
                SlotIndex = System.Array.IndexOf(slotIndexController.GetComponent<SlotIndexController>().SlotIndexes.ToArray(), thisSlot);
                break;
            case slotType.HOTBAR:
                SlotIndex = slotIndexController.GetComponent<SlotIndexController>().HotBarSlotIndexes.IndexOf(thisSlot);
                break;
            default: break;
        }
        Debug.Log(SlotIndex);
        UpdateSlotUI();
    }

    private enum slotType
    {
        INVENTORY,HOTBAR
    }

    public abstract void OnDrop(PointerEventData eventData);
    public abstract void UpdateSlotUI();
    protected virtual void EnableSlotUI(bool enable) => itemIconImage.enabled = enable;

}

