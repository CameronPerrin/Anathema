using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar;
    private float maxStamina = 100;
    private float currentStamina;
    public int waitTillRegen = 1;
    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    public static StaminaBar instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }

    public bool UseStamina(float amount)
    {
        if(currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            staminaBar.value = currentStamina;

            if (regen != null)
                StopCoroutine(regen);

            regen = StartCoroutine(RegenStamina());
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(waitTillRegen);

        while(currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 100;
            staminaBar.value = currentStamina;
            yield return regenTick;
        }
        regen = null;
    }

}
