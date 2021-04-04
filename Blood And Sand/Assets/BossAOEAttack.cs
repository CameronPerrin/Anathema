using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAOEAttack : MonoBehaviour
{
    [SerializeField] private GameObject circleParticleObject;
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private float timeUntilExplode;
    private bool minesCanAtk = false;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(minesAttackSpawn());

    }

    private void FixedUpdate()
    {
        if (minesCanAtk)
        {
            GameObject attackHitbox = Instantiate(attackPrefab, circleParticleObject.transform.position, transform.rotation * Quaternion.Euler(0, 0, -90));
            //attackHitbox.transform.localScale = new Vector3(2, 2, 2);
            minesCanAtk = false;
            Destroy(this.gameObject);
        }
    }

    private IEnumerator minesAttackSpawn()
    {
        circleParticleObject.transform.position = this.transform.position;
        //yield return new WaitForSeconds(0.25f);
        circleParticleObject.GetComponent<Renderer>().enabled = true;
        //circleParticleObject.SetActive(true);
        //CheckForPlayers();
        yield return new WaitForSeconds(timeUntilExplode);
        circleParticleObject.GetComponent<Renderer>().enabled = false;
        //circleParticleObject.SetActive(false);
        minesCanAtk = true;
    }


}
