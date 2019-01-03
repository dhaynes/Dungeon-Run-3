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
    public float cooldownBetweenClicks = 0.5f;
    public bool coolingDown = false;


    private float _meterFillAmt;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerAction()
    {
        if (coolingDown) return;
        if (GameController.instance.staminaMeter.stamina <= Cost) return;

        GameController.instance.staminaMeter.stamina -= Cost;

        InitiateSuccessfulAction();

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

        //apply cooldown
        coolingDown = true;

        Invoke("EndCooldown", cooldownBetweenClicks);
    }

    private void EndCooldown()
    {
        coolingDown = false;
    }

    private void DoAttack()
    {
        GameController.instance.hero.Attack();
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
