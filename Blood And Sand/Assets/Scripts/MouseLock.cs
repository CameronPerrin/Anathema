using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLock : MonoBehaviour
{
    public bool paused = false;
    public GameObject pauseScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //SUPER rough pause menu so I can hit buttons in the test scene
        if(Input.GetKeyDown(KeyCode.Escape))
        {   
            paused = !paused;
            if(paused)
            {
                pauseScreen.SetActive(true);
                this.GetComponent<PlayerMovementController>().isPaused = true;
                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);

            }
            else if(!paused)
            {
                pauseScreen.SetActive(false);
                this.GetComponent<PlayerMovementController>().isPaused = false;
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        if(!paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            this.GetComponent<PlayerMovementController>().isPaused = false;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if(paused)
        {
            Cursor.lockState = CursorLockMode.None;
            this.GetComponent<PlayerMovementController>().isPaused = true;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

    }
}
