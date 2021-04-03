using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyCounter : MonoBehaviour
{
    public int Money = 0;
    public TMP_Text MoneyText;
    private void Awake()
    {
        MoneyText.text = "Essence: " + Money.ToString();
    }

    public void addMoney(int addedMoney)
    {
        Money += addedMoney;
        MoneyText.text = "Essence: " + Money.ToString();
    }

}
