using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Text;
using UnityEngine.UI;
using TMPro;

public class InventoryController : MonoBehaviour
{

	private TMP_Text tempInvText;

	private Vector3 moveBy;
	private int itemCount = 0;
	private int saveCount = 1;
    // The weapon the player has.
    public GameObject MainHandWeapon;

    //where it loads inventory
    public Transform inventoryPanel;

    public List<GameObject> inventoryList;


	private Player_Data CreateSaveGameObject()
	{
		Player_Data data = new Player_Data();
	    data.object_name = MainHandWeapon.name;

	    data.attack = MainHandWeapon.GetComponent<WeaponStats>().attack;
	    data.item_value = MainHandWeapon.GetComponent<WeaponStats>().item_value;
		data.attack_speed = MainHandWeapon.GetComponent<WeaponStats>().attack_speed;
		data.crit_chance = MainHandWeapon.GetComponent<WeaponStats>().crit_chance;
		data.range = MainHandWeapon.GetComponent<WeaponStats>().range;
		data.item_type = MainHandWeapon.GetComponent<WeaponStats>().item_type;

	  return data;
	}

	private int GetInvNumber(){
		var fileNames = Directory.GetFiles(Application.persistentDataPath, "*.dat");

	    foreach(var fileName in fileNames)
	    {
	       saveCount++;
	    }
	    return saveCount;
	}

    public void Save()
    {
        //float test = 50;

    	Player_Data data1 = CreateSaveGameObject();

       // Debug.Log(Application.persistentDataPath);
		//Debug.Log(MainHandWeapon.GetInstanceID());


        //Serialize
		BinaryFormatter bf = new BinaryFormatter();

        // Stream the file with a File Stream. (Note that File.Create() 'Creates' or 'Overwrites' a file.)
        FileStream file = File.Create(Application.persistentDataPath + "/PlayerData"+GetInvNumber()+".dat");
        // Create a new Player_Data.
     
        //Save the data.



        //Serialize the file
        bf.Serialize(file, data1);
        //streamer.Seek(0, SeekOrigin.Begin);

        //Save to disk
        //file.Write(streamer.GetBuffer(), 0, streamer.GetBuffer().Length);

        // Close the file to prevent any corruptions
        file.Close();

        //string result = XElement.Parse(Encoding.ASCII.GetString(streamer.GetBuffer()).Replace("\0", "")).ToString();
        //Debug.Log("Serialized Result: " + result);

    }

    public void LoadIntoHand(){
    	if(File.Exists(Application.persistentDataPath + "/PlayerData.dat")){
    		BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
			Player_Data data = (Player_Data)bf.Deserialize(file);
			file.Close();

			Debug.Log(data.object_name);
			//find the prfab and store it in a variable to move to the palyer
			GameObject LoadedWeapon = Instantiate(Resources.Load("prefabs/items/"+data.object_name)) as GameObject;

			//give it to the player having issues with 
				//Setting the parent of a transform which resides in a Prefab Asset is disabled to prevent data corruption
			Debug.Log(this.gameObject);
			// LoadedWeapon.transform.parent = this.transform.GetChild(2);
   //          LoadedWeapon.transform.position = this.transform.GetChild(2).position;
   //          LoadedWeapon.transform.rotation = this.transform.GetChild(2).rotation;
			//give it stats from the loaded file
            LoadedWeapon.GetComponent<WeaponStats>().item_value = data.item_value;
            LoadedWeapon.GetComponent<WeaponStats>().attack = data.attack;
            LoadedWeapon.GetComponent<WeaponStats>().attack_speed = data.attack_speed;
            LoadedWeapon.GetComponent<WeaponStats>().crit_chance = data.crit_chance;
            LoadedWeapon.GetComponent<WeaponStats>().range = data.range;
            LoadedWeapon.GetComponent<WeaponStats>().defense = data.defense;
            LoadedWeapon.GetComponent<WeaponStats>().magic_defense = data.magic_defense;
            LoadedWeapon.GetComponent<WeaponStats>().move_speed = data.move_speed;
            LoadedWeapon.GetComponent<WeaponStats>().item_type = data.item_type;

    	}
	}

	public void LoadIntoInventory(){

		var fileNames = Directory.GetFiles(Application.persistentDataPath, "*.dat");

	    foreach(var fileName in fileNames)
	    {
	    	itemCount++;
	    	//old inv panel spawning
			// GameObject invPanel = Instantiate(Resources.Load("prefabs/ItemHolder")) as GameObject;
			// invPanel.transform.SetParent(inventoryPanel);
			// moveBy = new Vector3 ((Screen.width * 0.5f)+(itemCount*110), Screen.height * 0.5f, 0);
			// invPanel.transform.position = moveBy;
	    	//Debug.Log(fileName);
	    	if(File.Exists(fileName)){
	    		BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(fileName, FileMode.Open);
				Player_Data data = (Player_Data)bf.Deserialize(file);
				file.Close();
				//how we used to load in the actual 3d object.
				//GameObject LoadedItem = Instantiate(Resources.Load("prefabs/items/"+data.object_name)) as GameObject;
				//LoadedItem.transform.SetParent(inventoryPanel);
				GameObject LoadedItem = Instantiate(Resources.Load("prefabs/invItem")) as GameObject;
				LoadedItem.transform.SetParent(inventoryPanel);
				moveBy = new Vector3 (580f, (590f-(itemCount*55)), 0);
				LoadedItem.transform.position = moveBy;
				tempInvText = LoadedItem.GetComponentInChildren<TextMeshProUGUI>();
				tempInvText.text = data.object_name;
				Debug.Log(data.object_name);

    		}


	    }
	    itemCount = 0;


	}
}
[System.Serializable]
class Player_Data
{
	public int inv_number;

    public string object_name;


    //for all items
    //[DataMember]
	public float item_value;

	//for weapon items	
	//[DataMember]
	public float attack;// 0 to 100 damage per hit
	//[DataMember]
	public float attack_speed;//0 to 10 times per second
	//[DataMember]
	public float crit_chance;//0 to 100% chance
	//[DataMember]
	public float range; //0 to 100 meters

	//for armor
	//[DataMember]
	public float defense;
	//[DataMember]
	public float magic_defense;

	//for boots
	//[DataMember]
	public float move_speed;

	//item type
	//[DataMember]
	public int item_type;
	//1. Melee
	//2. Armor
	//3. boots
	//4. Ranged
	//5. Magic

}