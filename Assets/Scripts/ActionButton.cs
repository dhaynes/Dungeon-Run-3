using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButton : MonoBehaviour
{
    public enum ActionType { Dodge, Jump, Attack, Magic };
    public ActionType actionType;

    [Space]
    public TextMeshProUGUI Label;
    public Slider Meter;

    [Space]
    public float MaxCharge;
    public float CurrentCharge;
    public float Cost;

    private float _meterFillAmt;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CurrentCharge += Time.deltaTime * 10;

        if (CurrentCharge > MaxCharge)
        {
            CurrentCharge = MaxCharge;
        }

        Meter.value = CurrentCharge / MaxCharge;

    }

    public void TriggerAction()
    {
        bool tooSoon = false;
        if (CurrentCharge < Cost) tooSoon = true;

        CurrentCharge -= Cost;

        if (CurrentCharge < 0) CurrentCharge = 0;

        if (!tooSoon) Debug.Log("Success!"); else Debug.Log("Fail!");

        GameController.instance.Hero.Attack();

        GameController.instance.Enemy.TakeDamage(50);

    }

    private void HandlePointerDown()
    {
        Debug.Log("Pointer is down!");
    }

    private void OnValidate()
    {
        Label.text = actionType.ToString();
    }
}
