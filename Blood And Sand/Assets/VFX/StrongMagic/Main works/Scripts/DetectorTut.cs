using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LinkTut
{
    public GameObject link;
    public ConnectorTut connector;
    public string targetName;
}

public class DetectorTut : MonoBehaviour
{
    public GameObject linkPrefab;
    public GameObject impactPrefab;

    private List<LinkTut> linksList = new List<LinkTut>();

    void OnTriggerEnter (Collider co)
    {
        if(co.gameObject.tag == "Enemy")
        {
            if(linkPrefab != null)
            {
                LinkTut newLink = new LinkTut(){link = Instantiate (linkPrefab) as GameObject};
                newLink.connector = newLink.link.GetComponent<ConnectorTut>();
                newLink.targetName = co.name;
                linksList.Add(newLink);

                if(newLink.connector != null)
                    newLink.connector.MakeConnection(transform.position, co.transform.position);
            }

            if(impactPrefab != null)
            {
                var impactObj = Instantiate (impactPrefab, co.transform.position, Quaternion.identity) as GameObject;
                Destroy (impactObj, 2);
            }
        }
    }

    void OnTriggerStay (Collider co)
    {
        if(linksList.Count >0)
        {
            for(int i=0; i< linksList.Count; i++)
            {
                if(co.name == linksList[i].targetName)
                    linksList[i].connector.MakeConnection(transform.position, co.transform.position);
            }
        }
    }

    void OnTriggerExit (Collider co)
    {
        if(linksList.Count >0)
        {
            for(int i=0; i< linksList.Count; i++)
            {
                if(co.name == linksList[i].targetName)
                    Destroy(linksList[i].link);
            }
        }
    }
}
