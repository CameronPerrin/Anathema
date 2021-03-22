using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveWithKeyPress : MonoBehaviour
{
    [SerializeField] private KeyCode keyCode = KeyCode.None;
    [SerializeField] private GameObject objectToToggle = null;

    private void Update()
    {
        if(Input.GetKeyDown(keyCode))
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
        }
    }

}
