using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using Photon;
using UnityEngine.SceneManagement;

public class Combat : MonoBehaviourPun
{

	private PhotonView PV;
	public GameObject attackPrefab;
	public GameObject shootPoint;
    public GameObject CurrentPlayer;
	public float attackSpeed = 0;
	public float attackTimer = 0;
	public GameObject weap;
	public bool isPaused;
    
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
						Debug.Log(weap.name);
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
		float critChance = weap.GetComponent<WeaponStats>().crit_chance;
		float bleedChance = weap.GetComponent<WeaponStats>().bleed_chance;
		attackSpeed = weap.GetComponent<WeaponStats>().attack_speed;
		attackTimer = attackSpeed;
		float rand = UnityEngine.Random.Range(0.01f, 1.0f);
    	GameObject attackHitbox = Instantiate(attackPrefab, shootPoint.transform.position, Quaternion.identity);
		attackHitbox.GetComponent<bulletScript>().type = weap.GetComponent<WeaponStats>().item_type;
		if( rand <= critChance){
			attackHitbox.GetComponent<bulletScript>().dmg = weap.GetComponent<WeaponStats>().attack * 2;
		}
		if(rand <= bleedChance){
			attackHitbox.GetComponent<bulletScript>().dmg = weap.GetComponent<WeaponStats>().attack;
			attackHitbox.GetComponent<bulletScript>().DOT = true;
		}
		else
			attackHitbox.GetComponent<bulletScript>().dmg = weap.GetComponent<WeaponStats>().attack;
        //attackHitbox.GetComponent<Rigidbody>().velocity = CurrentPlayer.transform.GetChild(1).GetComponent<Rigidbody>().velocity;
        attackHitbox.transform.parent = CurrentPlayer.transform;
    }
}
