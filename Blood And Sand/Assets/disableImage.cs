using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class disableImage : MonoBehaviour
{
    public Image img;
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneAt(0)){
            img.enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name != "MainMenu"){
            img.enabled = true;
            Destroy(GetComponent<disableImage>());
        }
    }
}
