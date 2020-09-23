using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadoutController : MonoBehaviour
{
    //define player prefab so I can give him the weapon
    public GameObject player;

    //define prefabs that I can spawn
    public GameObject melee;
    public GameObject ranged;
    public GameObject magic;

    //set the prefab we are using
    public GameObject equipped;

    //used to make the equipped gameObject a child of the player prefab
    //[HideInInspector]
    //public GameObject spawned;

    //public variables that are changed when selecting an equiped weapon
    //set stats of weapon here

    //set model of weapon here (for now choose the prefab)
    //select weapon wanted in the room viewing panel using buttons. runa  function that set's a choice variable to true;
    //in the future we will set all the stats and model and choice here when equipping a weapon in the inventory.
    public int choice;

    // Start is called before the first frame update

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        equipped = melee;


        //instantiate chosen prefab structure
        //spawned = Instantiate(equipped, new Vector3(0, 0, 1), Quaternion.identity);
        //set model of the weapon to the one that is associate

        //put the new object on the player
        //spawned.transform.parent = player.transform;
    }


    public void MeleeChosen()
    {
        choice = 1;
        equipped = melee;
    }

    public void MagicChosen()
    {
        choice = 2;
        equipped = magic;
    }

    public void RangedChosen()
    {
        choice = 3;
        equipped = ranged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
