using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseHierarchyScript : MonoBehaviour
{


    public bool menuPause, inventoryPause, charPanelPause, chatPause, closeAllPanelsButMenu;     
    void Update()
    {
        // if(menuPause){
            
        // }
    }

    public void setPauseActive(int c)
    {
        switch(c){
            case 1:
                if(!menuPause){
                    // Activate pause menu
                    menuPause = true;
                    pauseThings(true);
                    // unpause everything else since menu is priority
                    
                }
                else{
                    // de-activate pause menu
                    if(inventoryPause || charPanelPause || chatPause)
                        menuPause = false;
                    else{
                        menuPause = false;
                        pauseThings(false);
                    }
                }
            break;

            case 2:
                if(!inventoryPause){
                    // Activate Inventory menu
                    inventoryPause = true;
                    pauseThings(true);
                }
                else{
                    // de-activate Inventory menu
                    if(menuPause || charPanelPause || chatPause)
                        inventoryPause = false;
                    else{
                        inventoryPause = false;
                        pauseThings(false);
                    }
                }
            break;

            case 3:
                if(!charPanelPause){
                    // Activate Character Panel menu
                    charPanelPause = true;
                    pauseThings(true);
                }
                else{
                    // de-activate Character Panel menu
                    if(inventoryPause || menuPause || chatPause)
                        charPanelPause = false;
                    else{
                        charPanelPause = false;
                        pauseThings(false);
                    }
                }
            break;

            case 4:
                if(!chatPause){
                    // Activate Chat menu
                    chatPause = true;
                    pauseThings(true);
                }
                else{
                    // de-activate Chat menu
                    if(inventoryPause || menuPause || charPanelPause)
                        chatPause = false;
                    else{
                        chatPause = false;
                        pauseThings(false);
                    }
                }
            break;

            default:
            break;
        }
    }

    public void pauseThings(bool b)
    {
        // PAUSE ALL THINGS
        if(b){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            this.GetComponent<PlayerMovementController>().isPaused = true;
            this.GetComponent<PlayerDash>().isPaused = true;
            this.GetComponent<Combat>().isPaused = true;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            closeAllPanelsButMenu = false;
        }
        // UNPAUSE ALL THINGS
        else{
            Cursor.lockState = CursorLockMode.Locked;
            this.GetComponent<PlayerMovementController>().isPaused = false;
            this.GetComponent<Combat>().isPaused = false;
            this.GetComponent<PlayerDash>().isPaused = false;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
