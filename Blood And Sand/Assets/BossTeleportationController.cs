using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleportationController : MonoBehaviour
{
    public GameObject[] portals;
    public GameObject[] boss;
    void Awake()
    { 
        portals = GameObject.FindGameObjectsWithTag("Boss_Portal");
        //portals[0].GetComponent<BossPortal>().isVacant = false;

    }

    public void setAllPortalsVacant()
    {
        //Debug.Log("Portal Count: " + portals.Length);
        for (int i = 0; i < portals.Length; i++)
        {
            portals[i].GetComponent<BossPortal>().isVacant = true;
        }
    }

    public int findVacantPortal()
    {

        for (int i = 0; i < portals.Length; i++)
        {
            if(portals[i].GetComponent<BossPortal>().isVacant == true)
            {
                return i;
            }
        }

        return 0;
    }
}
