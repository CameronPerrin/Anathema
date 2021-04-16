using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    //Gets properties from movement script.
    PlayerMovementController moveScript;


    public float dashSpeed;
    public float dashTime;
    public bool isPaused = false;
    [SerializeField] private int staminaUsage = 20;


    void Start()
    {
        moveScript = GetComponent<PlayerMovementController>();
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.F) && !isPaused)
        {
            if(StaminaBar.instance.UseStamina((float)staminaUsage))
            {
                //Couroutine is used to model behavior over several frames
                StartCoroutine(Dash());
            }

        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        while(Time.time < startTime + dashTime)
        {
            //Gets the movement direction from player and muliply by its dash speed and time.
            moveScript.controller.Move(moveScript.moveDir * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
