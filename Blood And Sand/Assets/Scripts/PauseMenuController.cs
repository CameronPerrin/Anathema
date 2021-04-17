using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class PauseMenuController : MonoBehaviour
{

	//public GameObject myCanvas;

	public int Money;
	public TMP_Text MoneyText;
	public GameObject InventoryMenu;
	public GameObject MainPauseMenu;
	public GameObject SettingsMenu;
	//public GameObject InventoryMenu;

	public TMP_Dropdown ResDropdown;
	public Resolution[] resolutions;
	public TMP_Dropdown QualityDropdown;
	public string[] QualityLevels;


	void Awake()
	{
		//DontDestroyOnLoad(this.gameObject);
		Money = 10000;
		resolutions = Screen.resolutions;

		// Print the resolutions
		//foreach (var res in resolutions)
		//{
		//	Debug.Log(res.width + "x" + res.height + " : " + res.refreshRate);
		//}
		PopulateResDropdown(ResDropdown, resolutions);
		ResDropdown.onValueChanged.AddListener(delegate {
			DropdownValueChanged(ResDropdown);
		});

		QualityLevels = QualitySettings.names;

		// Print the resolutions
		// foreach (var name in QualityLevels)
		// {
		// 	Debug.Log(name);
		// }
		PopulateQualityDropdown(QualityDropdown, QualityLevels);
		QualityDropdown.onValueChanged.AddListener(delegate {
			QualityDropdownValueChanged(QualityDropdown);
		});
	}

	void PopulateQualityDropdown(TMP_Dropdown dropdown, string[] QualityLevels)
	{
		List<string> options = new List<string>();
		foreach (var name in QualityLevels)
		{
			options.Add(name);
		}
		dropdown.ClearOptions();
		dropdown.AddOptions(options);
	}

	void PopulateResDropdown(TMP_Dropdown dropdown, Resolution[] resolutions)
	{
		List<string> options = new List<string>();
		foreach (var res in resolutions)
		{
			options.Add(res.width + "x" + res.height + " @ " + res.refreshRate + "hz"); // Or whatever you want for a label
		}
		dropdown.ClearOptions();
		dropdown.AddOptions(options);
	}

	public void DropdownValueChanged(TMP_Dropdown dropdown)
	{
		Debug.Log("res selected: "+ resolutions[dropdown.value]);
		//access resolutions[value] and change res
		Screen.SetResolution(resolutions[dropdown.value].width, resolutions[dropdown.value].height, true, resolutions[dropdown.value].refreshRate);
	}

	public void QualityDropdownValueChanged(TMP_Dropdown dropdown)
	{
		Debug.Log("Quality selected: " + QualityLevels[dropdown.value]);
		//change quality accordingly
		QualitySettings.SetQualityLevel(dropdown.value);
	}

	public void SetLevel(float sliderValue)
	{
		AudioListener.volume = sliderValue;
	}

	public void OpenInv()
	{
		InventoryMenu.SetActive(true);
		MainPauseMenu.SetActive(false);
		MoneyText.text = "Gil: " + Money.ToString();
	}

	public void CloseInv()
	{
		MainPauseMenu.SetActive(true);
		InventoryMenu.SetActive(false);
		foreach (Transform child in InventoryMenu.transform.GetChild(0)) {
     		GameObject.Destroy(child.gameObject);
 		}
	}

	public void OpenSettings()
	{
		MainPauseMenu.SetActive(false);
		SettingsMenu.SetActive(true);
	}

	public void CloseSettings()
	{
		MainPauseMenu.SetActive(true);
		SettingsMenu.SetActive(false);
	}

	public void quit()
    {
		#if UNITY_EDITOR
				 UnityEditor.EditorApplication.isPlaying = false;
		#else
				Application.Quit();
		#endif
	}

}
