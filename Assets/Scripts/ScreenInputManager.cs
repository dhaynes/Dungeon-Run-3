using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenInputManager : MonoBehaviour
{
    private void Awake()
    {
        SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
    }

    private void SwipeDetector_OnSwipe(SwipeData data)
    {
        switch (data.Direction)
        {
            case SwipeDirection.Left:
                {
                    GameController.instance.dodgeButton.TriggerAction();
                    break;
                }
            case SwipeDirection.Tap:
                {
                    GameController.instance.attackButton.TriggerAction();
                    break;
                }
            case SwipeDirection.Up:
                {
                    GameController.instance.jumpButton.TriggerAction();
                    break;
                }
        }
    }
}
