using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaMeter : MonoBehaviour
{
    public Slider meter;
    public float maxStamina = 100f;
    public float currentStamina;

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = 5f;
        UpdateMeter();
    }

    // Update is called once per frame
    void Update()
    {
        currentStamina += Time.deltaTime * 10f;

        UpdateMeter();
    }

    private void UpdateMeter()
    {
        meter.value = currentStamina / maxStamina;
    }
}
