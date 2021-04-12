using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
	public List<Transform> weapons;
	public int itemIndex;
	public TextMesh weaponText;
    // Start is called before the first frame update

	//add another pass in called rarity and have the random range change based on rarity
	//rarity can be a default variable that is part of different weapons
	//this would be something we set manually absed on how the weapon looks
	void giveStats(GameObject SpawnedWeapon){
		//set stats
		//SpawnedWeapon.GetComponent<WeaponStats>().attack = Mathf.Floor(Random.Range(0f, 100f));
		//SpawnedWeapon.GetComponent<WeaponStats>().attack_speed = Random.Range(0f, 1f);
		//SpawnedWeapon.GetComponent<WeaponStats>().crit_chance = Random.Range(0f, 1f);
		//SpawnedWeapon.GetComponent<WeaponStats>().range = Random.Range(0f, 0.01f);
		//set item type (all melee for now)
		//SpawnedWeapon.GetComponent<WeaponStats>().item_type = 1;
		//math out value
		SpawnedWeapon.GetComponent<WeaponStats>().item_value = Random.Range(50, 1000);;
	}

	void showStats(GameObject SpawnedWeapon) {
		weaponText.text = SpawnedWeapon.GetComponent<WeaponStats>().name + "\n"
						+ SpawnedWeapon.GetComponent<WeaponStats>().item_value + "Gil\n"
						+ SpawnedWeapon.GetComponent<WeaponStats>().attack + "ATK";
	}

    void Start()
    {
    	//put each child object into a list
        foreach (Transform child in transform.GetChild(0))
		{
 			weapons.Add(child);
 			//Debug.Log(child);
		}
		//choose one of the items in the list
		itemIndex = Random.Range(0, weapons.Count);
		//set that random item as active
		weapons[itemIndex].gameObject.SetActive(true);
		//give that item random variables
		giveStats(weapons[itemIndex].gameObject);
		//set weapon to weaponShowing in interact.cs
		this.GetComponent<Interact>().weaponShowing = weapons[itemIndex];
		//grab the canvas above the weapon to display cost and the dmg
		showStats(weapons[itemIndex].gameObject);
		//maybe eventually show how much money you have to spend above it too?

		this.GetComponent<Interact>().weaponShowingItemSlot.item = Object.Instantiate(Resources.Load<InventoryItem>("Items/Equipments/" + weapons[itemIndex].name));
		this.GetComponent<Interact>().weaponShowingItemSlot.quantity = 1;

		//weapons[itemIndex].name
	}

    //eventually we will have this for all items
    //we will also make cooler items have better stats
}
