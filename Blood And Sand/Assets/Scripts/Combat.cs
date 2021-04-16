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
	public Camera cam;
	public GameObject attackPrefab;
	public GameObject meleeSlash;
	public GameObject meleeStab;
	public GameObject magicFast;
	public GameObject magicStrong;
	public GameObject shootPoint;
	public GameObject stabPoint;
	public GameObject magicPoint;
	public GameObject CurrentPlayer;

	// VFX
	public GameObject attackVFX;
	public GameObject impactVFX;

	public float attackSpeed = 0;
	public float attackTimer = 0;
	GameObject weap;
	public bool isPaused;
	GameObject attackHitbox;

	[HideInInspector] public int critMultiplier = 1;
    
	void Start()
	{
		PV = GetComponent<PhotonView>();
        CurrentPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
		cam = Camera.main;
	}

	void Update()
	{
		if(Input.GetMouseButton(0) && SceneManager.GetActiveScene().buildIndex != 1 && attackTimer <= 0)
    	{
    		if(PV.IsMine)
    		{
				if(!isPaused)
				{
    				
					try{
						weap = this.gameObject.transform.Find("Capsule/weaponHolder").gameObject.transform.GetChild(0).gameObject;
						//weap = this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
						Attack();
					}
					catch (Exception e){
						//Debug.Log("Can't find item, are you sure player has it equipped?");
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

		// Assign VFX
		attackVFX = weap.GetComponent<WeaponStats>().attack_vfx;
		impactVFX = weap.GetComponent<WeaponStats>().impact_vfx;

		// Tell the damage prefab what type of specials it has, and calculate the chances of secondary active stats activating
		attackPrefab.GetComponent<bulletScript>().type = weap.GetComponent<WeaponStats>().item_type;
		if(rand <= critChance){
			critMultiplier = 2;
		}
		else
			critMultiplier = 1;
		
		//Debug.Log(bleedChance);
		if(rand <= bleedChance){
			attackPrefab.GetComponent<bulletScript>().DOT = true;
			//Debug.Log("Should bleed...");
		}
		else
			attackPrefab.GetComponent<bulletScript>().DOT = false;
			
		// set final damage for attack
		attackPrefab.GetComponent<bulletScript>().dmg = weap.GetComponent<WeaponStats>().attack * critMultiplier;
		// Instatiate attack


		if(weap.GetComponent<WeaponStats>().item_type == 6) // Stab
		{
			attackHitbox = Instantiate(attackPrefab, stabPoint.transform.position, stabPoint.transform.rotation);
			Instantiate(attackVFX, stabPoint.transform.position, stabPoint.transform.rotation);
		}
		else if (weap.GetComponent<WeaponStats>().item_type == 1) // Slash
        {
			attackHitbox = Instantiate(attackPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
			Instantiate(attackVFX, shootPoint.transform.position, shootPoint.transform.rotation);
		}
		else // Magic attacks
        {
			attackHitbox = Instantiate(attackPrefab, magicPoint.transform.position, cam.transform.rotation);
			Instantiate(attackVFX, magicPoint.transform.position, cam.transform.rotation);
		}

		// impact VFX
		attackHitbox.GetComponent<bulletScript>().impactVFX = impactVFX;

		//shootPoint.transform.position = tempShootPoint;

        // [OLD] -- >attackHitbox.GetComponent<Rigidbody>().velocity = CurrentPlayer.transform.GetChild(1).GetComponent<Rigidbody>().velocity;

		if(weap.GetComponent<WeaponStats>().item_type == 2 || weap.GetComponent<WeaponStats>().item_type == 7){
			RaycastHit hit;
			Ray pointing = cam.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit)){
				//GameObject attackHitbox = Instantiate(attackPrefab, shootPoint.transform.position, faceTo);
				attackHitbox.GetComponent<bulletScript>().aim = hit.point;
			}
		}
		else
        	attackHitbox.transform.parent = CurrentPlayer.transform;
    }
}
