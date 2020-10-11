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

            }
            else if(!paused)
            {
                pauseScreen.SetActive(false);
            }
        }

        if(!paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if(paused)
        {
            Cursor.lockState = CursorLockMode.None;
        }

    }
}
