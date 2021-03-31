using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyCounter : MonoBehaviour
{
    public int Money;
    public TMP_Text MoneyText;
    private void Awake()
    {
        Money = 1000;

        MoneyText.text = "Essence: " + Money.ToString();
    }

}
