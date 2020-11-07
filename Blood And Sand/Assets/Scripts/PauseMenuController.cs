using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{

	public GameObject InventoryMenu;
	public GameObject MainPauseMenu;

	public void OpenInv()
	{
		InventoryMenu.SetActive(true);
		MainPauseMenu.SetActive(false);
	}
}
