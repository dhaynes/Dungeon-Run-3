using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMeter : MonoBehaviour
{
    public Slider meter;

    public Animator animator;

	public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }


}
