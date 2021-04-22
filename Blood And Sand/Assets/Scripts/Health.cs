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
    public GameObject sprintBar;

    public GameObject deathObj;
    public GameObject bloodVFX;
    public GameObject bloodSpotInstLocation;

    public GameObject FloatingTextPrefab;
    public float dmgTake; // Placeholder damage for when we add in weapons
    public float dmgTemp;

    public Inventory inventory;
    GameObject removePlayer;

    bool isAlreadyDead = false;

    private void OnDisable()
    {
        if(removePlayer != null){
            removePlayer.GetComponent<deathScript>().killPlayer(this.gameObject);
        }
        if(PV.IsMine)
            GetComponent<ChatScript>().PV.RPC("sendChat", RpcTarget.All,"", $"<color=yellow>{PhotonNetwork.LocalPlayer.NickName} has left the room.</color>", true);
    }

    void Awake()
    {
        removePlayer = GameObject.Find("TheReaper");
        // if(SceneManager.GetActiveScene().buildIndex == 1){
        //     Destroy(wHp);
        //     Destroy(oHp);
        // }
    }

    void Start()
    {
    	PV = GetComponent<PhotonView>();
        health = maxHp;
        if(PV.IsMine)
    	{
    		wHp.SetActive(false);
            sprintBar.SetActive(true);
    	}
    	else{
    		oHp.SetActive(false);
            sprintBar.SetActive(false);
    	}
        InvokeRepeating ("RegenHealth", 0f, hpRegenSpeed);
    }

    void RegenHealth ()
    {
        if(health < maxHp){
            health += hpRegen;
            if(health > maxHp)
                health = maxHp;
            //overlayHealthBar.fillAmount = health/maxHp;
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
        overlayHealthBar.fillAmount = health/maxHp;
        turnCorrupted(isCorrupted);
    }

    public void TakeDamage(float dmage, int type, bool dot)
    {
        
        if(type == 1 || type == 6){ // physical defense
            //Debug.Log("PHYS DAMAGE");
            dmgTake = dmage - physicalDefense;
            if(dmgTake <= 0)
                dmgTake = 0;
            PV.RPC("Damage", RpcTarget.All, dmgTake);
        }
        else if(type == 2 || type == 7){ // magic defense
            dmgTake = dmage - magicDefense;
            if(dmgTake <= 0)
                dmgTake = 0;
            PV.RPC("Damage", RpcTarget.All, dmgTake);
        }
        if(dot){
            Debug.Log("DOT DAMAGE!");
            dmgTemp = dmage;
            dmgTake = (dmage - physicalDefense) / 4;
            if(dmgTake <= 0)
                dmgTake = 0;
            InvokeRepeating ("PhysicalDOTDmg", 0f, dotTimer);
        }
        
    }

    public void PhysicalDOTDmg()
    {
        if(dmgTemp > 0){
            //Debug.Log("Bleeding for " + dmg + " damage!");
            
            PV.RPC("Damage", RpcTarget.All, dmgTake);
            dmgTemp -= dmgTake;
        }
        else{
            this.CancelInvoke("PhysicalDOTDmg");
        }
    }

    [PunRPC]
    public void Damage(float dmage)
    {
        
        // Always show damage dealt first!
        if(FloatingTextPrefab)
        {
            ShowFloatingText(dmage);
        }
        // temp script container so as to not call get component more than ncessary
        var tempReapScript = removePlayer.GetComponent<deathScript>();
        //Debug.Log("Taking " + dmage + " damage.");
        health -= dmage;
        //Instantiate(bloodVFX, bloodSpotInstLocation.transform.position, Quaternion.identity); // spawn blood vfx
        if(health <= 0 && !isAlreadyDead){
            tempReapScript.killPlayer(this.gameObject);
            if(PV.IsMine)
                GetComponent<ChatScript>().PV.RPC("sendChat", RpcTarget.All,"", $"<color=#ff0a0a>{PhotonNetwork.LocalPlayer.NickName} has died.</color>", true);
        }
        if(PV.IsMine) {
            if(health <= 0 && !isAlreadyDead){
                tempReapScript.onDeath(); // Find obj, find script, call function
                isAlreadyDead = true;     
            }  
        }
        // update hpbar fill amount
        worldHealthBar.fillAmount = health/maxHp;
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting)
        {
            //this is my player!
            stream.SendNext(health);
            stream.SendNext(isCorrupted);
        }

        else if(stream.IsReading)
        {
            //everyone else
            health = (float)stream.ReceiveNext();
            isCorrupted = (bool)stream.ReceiveNext();
        }
    }

    // Spawn in damage text and makes sure that the text rotation is facing the camera
    // ** This might be changed later because text does not show sometimes in multiplayer session. **
    void ShowFloatingText(float dmage)
    {
        var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        go.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        //go.transform.LookAt(Camera.main.transform);
        go.GetComponent<TextMesh>().text = dmage.ToString();
    }

    private void OnTriggerEnter(Collider collision)
    {
        
        if(collision.gameObject.tag == "loot"){
            GameObject esse = collision.gameObject;
            //Debug.Log(pl.name);
            //moneyStorage = pl.transform.Find("InventoryManager/InventoryUI/InventoryCanvas/InventoryPanel/CurrencyCounter").gameObject;
            //Debug.Log(pl.name+" "+moneyStorage.name);
            //Debug.Log(moneyStorage.name);
            //Debug.Log(PV.IsMine);
            inventory.Money += esse.GetComponent<essenceScript>().essenceAmount;
            if(esse.GetComponent<essenceScript>().isCorruptedEssence){
                if(PV.IsMine){
                    //Debug.Log(PV.IsMine);
                }   
                if(isCorrupted == false && tag != "Corrupted_Player"){
                    tag = "Corrupted_Player";
                    isCorrupted = true;
                    //turnCorrupted(isCorrupted);
                    
                    //moveSpeed *= 2;   
                    } 
                }
            Destroy(esse.gameObject);
        }  
    }

    public void turnCorrupted(bool corr)
    {
        if(corr){
            transform.Find("playerModelUnity/body").gameObject.GetComponent<Renderer>().material.color = Color.red;
            //maxHp *= 2;
            health = maxHp;
            physicalDefense += 10;
            magicDefense += 10;
            isCorrupted = false;
        }
    }
}
