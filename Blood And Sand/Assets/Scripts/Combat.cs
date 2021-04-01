using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using Photon;
using UnityEngine.SceneManagement;

public class Combat : MonoBehaviour
{

	private PhotonView PV;
	public GameObject attackPrefab;
	public GameObject meleeSlash;
	public GameObject meleeStab;
	public GameObject magicFast;
	public GameObject magicStrong;
	public GameObject shootPoint;
    public GameObject CurrentPlayer;
	public float attackSpeed = 0;
	public float attackTimer = 0;
	GameObject weap;
	public bool isPaused;

	[HideInInspector] public int critMultiplier = 1;
    
	void Start()
	{
		PV = GetComponent<PhotonView>();
        CurrentPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0) && SceneManager.GetActiveScene().buildIndex != 1 && attackTimer <= 0)
    	{
    		if(PV.IsMine)
    		{
				if(!isPaused)
				{
    				
					try{
						weap = this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
						Attack();
					}
					catch (Exception e){
						Debug.Log("Can't find item, are you sure player has it equipped?");
					}
				}
    		}
    	}
		else{
			attackTimer -= Time.deltaTime;
		}
	}

    public void Attack()
    {
    	PV.RPC("RpcShoot", RpcTarget.All);
    }

    [PunRPC]
    void RpcShoot()
    {
		// Decide what type of weapon attack it is
		if(weap.GetComponent<WeaponStats>().item_type == 1)
			attackPrefab = meleeSlash;
		else if(weap.GetComponent<WeaponStats>().item_type == 6)
			attackPrefab = meleeStab;
		else if(weap.GetComponent<WeaponStats>().item_type == 2)
			attackPrefab = magicFast;
		else if(weap.GetComponent<WeaponStats>().item_type == 7)
			attackPrefab = magicStrong;

		// Secondary active stats
		float critChance = weap.GetComponent<WeaponStats>().crit_chance;
		float bleedChance = weap.GetComponent<WeaponStats>().bleed_chance;
		attackSpeed = weap.GetComponent<WeaponStats>().attack_speed;
		attackTimer = attackSpeed;
		float rand = UnityEngine.Random.Range(0.01f, 1.0f);

		

		// Tell the damage prefab what type of specials it has, and calculate the chances of secondary active stats activating
		attackPrefab.GetComponent<bulletScript>().type = weap.GetComponent<WeaponStats>().item_type;
		if(rand <= critChance){
			critMultiplier = 2;
			Debug.Log("Here comes the CRIT: " + attackPrefab.GetComponent<bulletScript>().dmg);
		}
		else
			critMultiplier = 1;
		if(rand <= bleedChance){
			attackPrefab.GetComponent<bulletScript>().DOT = true;
		}
		else
			attackPrefab.GetComponent<bulletScript>().DOT = false;
			
		attackPrefab.GetComponent<bulletScript>().dmg = weap.GetComponent<WeaponStats>().attack * critMultiplier;
		// Initiate attack
    	GameObject attackHitbox = Instantiate(attackPrefab, shootPoint.transform.position, Quaternion.identity);

        // [OLD] -- >attackHitbox.GetComponent<Rigidbody>().velocity = CurrentPlayer.transform.GetChild(1).GetComponent<Rigidbody>().velocity;

		if(weap.GetComponent<WeaponStats>().item_type == 2 || weap.GetComponent<WeaponStats>().item_type == 7){
			attackHitbox.transform.rotation = CurrentPlayer.transform.rotation;
		}
		else
        	attackHitbox.transform.parent = CurrentPlayer.transform;
    }
}
