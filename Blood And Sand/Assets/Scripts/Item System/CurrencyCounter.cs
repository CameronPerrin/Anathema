using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyCounter : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory;

    public TMP_Text MoneyText;


    private void LateUpdate()
    {
            MoneyText.text = "Essence: " + playerInventory.Money;
    }


}


