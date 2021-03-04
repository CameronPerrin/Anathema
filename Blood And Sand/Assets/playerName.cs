using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerName : MonoBehaviour
{
    public string pName;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void saveName(string name)
    {
        pName = name;
    }
}
