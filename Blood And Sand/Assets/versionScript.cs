using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class versionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(this.GetComponent<TMP_Text>().text = "ddadad");
        this.GetComponent<TMP_Text>().text = Application.version;
    }
    
}
