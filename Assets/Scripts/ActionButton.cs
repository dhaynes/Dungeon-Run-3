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
    public StaminaMeter staminaMeter;

    [Space]
    public float MaxCharge;
    public float CurrentCharge;
    public float Cost;

    [Space]
    public float ChargeSpeed = 1f;


    private float _meterFillAmt;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        CurrentCharge += (Time.deltaTime * Cost) * ChargeSpeed;

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

        if (!tooSoon) 
        {
            //Debug.Log("Success!");

            CurrentCharge -= Cost;
            if (CurrentCharge < 0) CurrentCharge = 0;

            InitiateSuccessfulAction();
        }
        else 
        {
            //Debug.Log("Fail!");
        }

    }

    private void InitiateSuccessfulAction()
    {
        switch (actionType)
        {
            case ActionType.Jump:
                DoJump();
                break;

            case ActionType.Dodge:
                DoDodge();
                break;

            case ActionType.Magic:
                DoAttack();
                break;

            default: //"Attack"
                DoAttack();
                break;
        }
    }

    private void DoAttack()
    {
        GameController.instance.hero.Attack();
        GameController.instance.enemyGroup.currentEnemy.TakeDamage(GameController.instance.hero.strength);
    }

    private void DoJump()
    {
        GameController.instance.hero.Jump();
    }

    private void DoDodge()
    {
        GameController.instance.hero.Dodge();
    }

    private void OnValidate()
    {
        Label.text = actionType.ToString();
    }
}
