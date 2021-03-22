using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rarity", menuName = "Item/Rarity")]
public class Rarity : ScriptableObject
{
    [SerializeField] private new string name = "New Rarity Name";
    [SerializeField] private Color textColor = new Color(1f, 1f, 1f, 1f);

    public string Name => name;
    public Color TextColour => textColor;
}
