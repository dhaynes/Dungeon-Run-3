using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventHelper : MonoBehaviour
{
    //Helper script that allows animation events to target the parent
    private void Awake()
    {

    }

    private void TriggerAnimEvent(string str)
    {
        transform.parent.gameObject.SendMessage(str);
    }
}
