using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ItemDestroyer : MonoBehaviour
{
    [SerializeField] private Inventory inventory = null;
    [SerializeField] private TextMeshProUGUI areYouSureText = null;

    private int slotIndex = 0;

    private void OnDisable() => slotIndex = -1;

    public Canvas destroyItemCanvas;
    void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneAt(0))
        {
            destroyItemCanvas.enabled = false;
        }

    }

    public void Activate(ItemSlot itemSlot, int slotIndex)
    {
        destroyItemCanvas.enabled = true;
        this.slotIndex = slotIndex;
        areYouSureText.text = $"Are you sure you wish to destroy {itemSlot.quantity}x {itemSlot.item.ColouredName}?";
        gameObject.SetActive(true);
    }

    public void Destroy()
    {
        Debug.Log("Destroy Clicked");
        inventory.ItemContainer.RemoveAt(slotIndex);
        gameObject.SetActive(false);
    }

}
