using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeEquipment : MonoBehaviour
{
    [SerializeField] private UI_CharacterEquipment uiCharacterEquipment;
    [SerializeField] private PlayerEquipment playerEquipment;

    // Start is called before the first frame update
    void Start()
    {
        uiCharacterEquipment.SetCharacterEquipment(playerEquipment);
    }
}
