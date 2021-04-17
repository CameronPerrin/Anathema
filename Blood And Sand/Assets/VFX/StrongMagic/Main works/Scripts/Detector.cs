using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link
{
    public GameObject link;
    public Connector connector;
    public string targetName;
}

public class Detector : MonoBehaviour
{
    public GameObject linkPrefab;
    public GameObject impactPrefab;

    private List<Link> linkList = new List<Link>();

    void OnTriggerEnter (Collider col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            if(linkPrefab != null)
            {            
                Link newLink = new Link(){link = Instantiate (linkPrefab) as GameObject};
                newLink.connector = newLink.link.GetComponent<Connector>();
                newLink.targetName = col.name;
                linkList.Add (newLink);

                if(newLink.connector != null)                
                    newLink.connector.MakeConnection(transform.position, col.transform.position);                
                else                
                    Debug.Log("No connector");                
            }
            else            
                Debug.Log("No link prefab");  

            if(impactPrefab != null)
            {
                var impactObj = Instantiate (impactPrefab, col.transform.position, Quaternion.identity) as GameObject;
                Destroy (impactObj, 2);
            }          
        }
    }

    void OnTriggerStay (Collider col)
    {
        if(linkList.Count>0)
        {
            for(int i=0; i<linkList.Count; i++)
            {
                if(col.name == linkList[i].targetName)
                    linkList[i].connector.MakeConnection(transform.position, col.transform.position);
            }
        } 
    }

    void OnTriggerExit (Collider col)
    {
        if(linkList.Count>0)
        {
            for(int i=0; i<linkList.Count; i++)
            {
                if(col.name == linkList[i].targetName)
                    Destroy(linkList[i].link);
            }
        }
    }

    public void DestroyAllLinks ()    
    {
        for(int i=0; i<linkList.Count; i++)
        {
            Destroy(linkList[i].link);
        }

        linkList.Clear();
    }

}
