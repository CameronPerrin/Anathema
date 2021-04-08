using System;


[Serializable]
public class ItemSlotSaveData
{
    public string ItemID;
    public int Amount;
	public Item_Data itemData;


	public ItemSlotSaveData(string id, int amount)
    {
        ItemID = id;
        Amount = amount;
    }
}

[Serializable]
public class ItemContainerSaveData
{
    public ItemSlotSaveData[] SavedSlots;
	public int Money;

    public ItemContainerSaveData(int numItems, int moneyAmount)
    {
        SavedSlots = new ItemSlotSaveData[numItems];
		Money = moneyAmount;
    }
}

[Serializable]
public class Item_Data
{
	public string object_name;
	//for all items
	//[DataMember]
	public int item_value;

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

	public float bleed_chance;
	//1. Melee
	//2. Armor
	//3. boots
	//4. Ranged
	//5. Magic

	public override bool Equals(object obj)
	{
		var other = obj as Item_Data;

		if (other == null)
			return false;

		if (item_value != other.item_value || attack != other.attack || attack_speed != other.attack_speed || crit_chance != other.crit_chance)
			return false;

		if(range != other.range || defense != other.defense || magic_defense != other.magic_defense || move_speed != other.move_speed || item_type != other.item_type || bleed_chance != other.bleed_chance)
        {
			return false;
        }

		return true;
	}

}