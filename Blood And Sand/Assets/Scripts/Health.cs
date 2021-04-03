using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon;
using UnityEngine.SceneManagement;


public class Health : MonoBehaviourPunCallbacks, IPunObservable
{

    // Start is called before the first frame update
	private PhotonView PV;
    public float maxHp = 100f;
    public float health;
    public float hpRegen = 0f;
    public float hpRegenSpeed = 1f;
    public float physicalDefense = 0;
    public float magicDefense = 0;
    public float dotTimer = 0.5f;
    public bool isCorrupted = false;
    public Image overlayHealthBar; 		// HP images for screenspace overlay
    public Image redScreenHealthBackdrop;
    public Image worldHealthBar;		// HP images for worldspace overlay
    public Image redWorldHealthBackdrop;
    public GameObject oHp; 				// canvas overlay
    public GameObject wHp; 				// canvas world

    public GameObject deathObj;
    public GameObject bloodVFX;
    public GameObject bloodSpotInstLocation;

    public GameObject FloatingTextPrefab;
    public float dmgTake; // Placeholder damage for when we add in weapons
    public float dmgTemp;


    void Awake()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1){
            Destroy(wHp);
            Destroy(oHp);
        }
    }

    void Start()
    {
    	PV = GetComponent<PhotonView>();
        health = maxHp;
        if(PV.IsMine)
    	{
    		wHp.SetActive(false);
    	}
    	else{
    		oHp.SetActive(false);
    	}
        InvokeRepeating ("RegenHealth", 0f, hpRegenSpeed);
    }

    void RegenHealth ()
    {
        if(health < maxHp){
            health += hpRegen;
            if(health > maxHp)
                health = maxHp;
            overlayHealthBar.fillAmount = health/maxHp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(health/maxHp != redWorldHealthBackdrop.fillAmount)
        {
            redWorldHealthBackdrop.fillAmount = Mathf.Lerp(redWorldHealthBackdrop.fillAmount, health/maxHp, Time.deltaTime * 4);
            redScreenHealthBackdrop.fillAmount = Mathf.Lerp(redScreenHealthBackdrop.fillAmount, health/maxHp, Time.deltaTime * 4);
        }
        
    }

    public void TakeDamage(float dmage, int type, bool dot)
    {
        
        if(type == 1){ // physical defense
            dmgTake = dmage - physicalDefense;
        }
        else if(type == 2){ // magic defense
            dmgTake = dmage - magicDefense;
        }
        if(dot){
            dmgTemp = dmage;
            dmgTake = (dmage - physicalDefense) / 4;
            InvokeRepeating ("PhysicalDOTDmg", 0f, dotTimer);
        }
        if(PV.IsMine){
            PV.RPC("Damage", RpcTarget.All);
        }
    }

    public void PhysicalDOTDmg()
    {
        if(dmgTemp > 0){
            //Debug.Log("Bleeding for " + dmg + " damage!");
            if(PV.IsMine){
                PV.RPC("Damage", RpcTarget.All, dmgTake);
            }
            dmgTemp -= dmgTake;
        }
        else{
            this.CancelInvoke("PhysicalDOTDmg");
        }
    }

    [PunRPC]
    public void Damage()
    {
        health -= dmgTake;
        Instantiate(bloodVFX, bloodSpotInstLocation.transform.position, Quaternion.identity); // spawn blood vfx
        if(health <= 0){
            GameObject.Find("TheReaper").GetComponent<deathScript>().killPlayer(this.gameObject);
        }
        if(PV.IsMine) {
            if(health <= 0){
                GameObject.Find("TheReaper").GetComponent<deathScript>().onDeath(); // Find obj, find script, call function     
            }  
        }
        overlayHealthBar.fillAmount = health/maxHp;
        worldHealthBar.fillAmount = health/maxHp;

        if(FloatingTextPrefab)
        {
            ShowFloatingText();
        }
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting)
        {
            //this is my player!
            stream.SendNext(health);
        }

        else if(stream.IsReading)
        {
            //everyone else
            health = (float)stream.ReceiveNext();
        }
    }

    // Spawn in damage text and makes sure that the text rotation is facing the camera
    // ** This might be changed later because text does not show sometimes in multiplayer session. **
    void ShowFloatingText()
    {
        var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        go.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        //go.transform.LookAt(Camera.main.transform);
        go.GetComponent<TextMesh>().text = dmgTake.ToString();
    }


}
