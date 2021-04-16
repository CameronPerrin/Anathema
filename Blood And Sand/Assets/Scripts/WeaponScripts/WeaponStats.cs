using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
	// VFX
	public GameObject attack_vfx;
	public GameObject impact_vfx;

	//for all items
	public int item_value;

	//for weapon items	
	public float attack;// 0 to 100 damage per hit
	public float attack_speed;//0 to 10 times per second
	public float crit_chance;//0 to 100% chance
	public float bleed_chance;//0 to 100% chance
	public float range; //0 to 100 meters

	//for armor
	public float defense;
	public float magic_defense;

	//for boots
	public float move_speed;

	//item type
	public int item_type;
	//1. Melee Slash
	//2. Magic Fast
	//3. Melee DOT
	//4. Magic DOT
	//5. Armor
	//6. Melee Stab
	//7. Magic Strong
	public bool isCorruptedEssence = false;

    void updateValue(){
    	//call this to change the value of the item based on the stats
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public Item_Data returnItemData()
    {
		Item_Data data = new Item_Data();
		data.object_name = name;
		data.attack = attack;
		data.item_value = item_value;
		data.attack_speed = attack_speed;
		data.crit_chance = crit_chance;
		data.range = range;
		data.item_type = item_type;
		data.magic_defense = magic_defense;
		data.move_speed = move_speed;
		data.bleed_chance = bleed_chance;

		return data;
	}
}
