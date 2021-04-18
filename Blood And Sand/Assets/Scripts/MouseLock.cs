using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class MouseLock : MonoBehaviourPun
{
    public bool paused = false;
    public GameObject pauseScreen;
    [SerializeField] private GameObject inventoryToToggle = null;
    [SerializeField] private GameObject equipmentToToggle = null;
    [HideInInspector]public PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if(PV.IsMine){
            pauseScreen = GameObject.FindGameObjectWithTag("PauseMenu");
            inventoryToToggle = GameObject.FindGameObjectWithTag("InventoryPanel");
            equipmentToToggle = GameObject.FindGameObjectWithTag("EquipmentPanel");
            pauseScreen.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(PV.IsMine){
        //pauseScreen = GameObject.FindGameObjectWithTag("PauseMenu");
        //SUPER rough pause menu so I can hit buttons in the test scene
        if (Input.GetKeyDown(KeyCode.Escape) && PV.IsMine)
        {
            
            paused = !paused;
            if(paused)
            {
                pauseScreen.SetActive(true);
                this.GetComponent<PlayerMovementController>().isPaused = true;
                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                this.GetComponent<Combat>().isPaused = true;
            }
            else if(!paused)
            {
                pauseScreen.SetActive(false);
                this.GetComponent<PlayerMovementController>().isPaused = false;
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                this.GetComponent<Combat>().isPaused = false;
            }
        }

        if (!paused && inventoryToToggle.transform.localScale == new Vector3(0, 0, 0) && equipmentToToggle.transform.localScale == new Vector3(0, 0, 0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            //this.GetComponent<PlayerMovementController>().isPaused = false;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            this.GetComponent<PlayerMovementController>().isPaused = false;
            this.GetComponent<Combat>().isPaused = false;
        }
        else if (!paused && (inventoryToToggle.transform.localScale == new Vector3(1, 1, 1) || equipmentToToggle.transform.localScale == new Vector3(1, 1, 1)))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            this.GetComponent<PlayerMovementController>().isPaused = true;
            this.GetComponent<Combat>().isPaused = true;
        }
        else if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
            this.GetComponent<PlayerMovementController>().isPaused = true;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }


        /*if(!paused && !inventoryToToggle.activeSelf && !equipmentToToggle.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            //this.GetComponent<PlayerMovementController>().isPaused = false;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            this.GetComponent<PlayerMovementController>().isPaused = false;
            this.GetComponent<Combat>().isPaused = false;
        }
        else if (!paused && (inventoryToToggle.activeSelf || equipmentToToggle.activeSelf))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            this.GetComponent<PlayerMovementController>().isPaused = true;
            this.GetComponent<Combat>().isPaused = true;
        }
        else if(paused)
        {
            Cursor.lockState = CursorLockMode.None;
            this.GetComponent<PlayerMovementController>().isPaused = true;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        } */
        }
    }
}
