using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaMeter : MonoBehaviour
{
    public Slider meter;
    public float maxStamina = 100f;
    public float stamina = 0f;
    public float chargeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        stamina = 5f;
        UpdateMeter();
    }

    // Update is called once per frame
    void Update()
    {
        stamina += Time.deltaTime * chargeSpeed;
        if (stamina > maxStamina) stamina = maxStamina;

        UpdateMeter();
    }

    private void UpdateMeter()
    {
        meter.value = stamina / maxStamina;
    }
}
