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
    public StaminaMeter staminaMeter;

    [Space]
    public float Cost;

    [Space]
    public float ChargeSpeed = 1f;
    public float cooldownBetweenClicks = 0.5f;
    public bool coolingDown = false;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        //CheckInput();
    }




    public void TriggerAction()
    {
        if (coolingDown) return;
        if (GameController.instance.staminaMeter.stamina <= Cost) return;

        GameController.instance.staminaMeter.stamina -= Cost;

        switch (actionType)
        {
            case ActionType.Attack:
                {
                    DoAttack();
                    break;
                }
            case ActionType.Dodge:
                {
                    DoDodge();
                    break;
                }
            case ActionType.Jump:
                {
                    DoJump();
                    break;
                }
            default:
                {
                    DoAttack();
                    break;
                }
        }

        
        //apply cooldown
        coolingDown = true;

        Invoke("EndCooldown", cooldownBetweenClicks);

    }

    private void EndCooldown()
    {
        coolingDown = false;
    }

    public void DoAttack()
    {
        GameController.instance.hero.Attack();
    }

    public void DoJump()
    {
        GameController.instance.hero.Jump();
    }

    public void DoDodge()
    {
        GameController.instance.hero.Dodge();
    }

    private void OnValidate()
    {
        Label.text = actionType.ToString();
    }
}
