using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
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
	//1. Melee
	//2. Magic
	//3. Melee DOT
	//4. Magic DOT
	//5. Armor

    void updateValue(){
    	//call this to change the value of the item based on the stats
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
