using UnityEngine;
using System.Collections;
using TMPro;
public class StatusEffectTextObject : PoolObject {

    public Animator animator;
    public TextMeshPro textMeshPro;
	
	void Awake () 
	{
        textMeshPro.text = "";
	}
	
	public override void OnObjectReuse()
	{
        animator.Play("ShowEffect", -1, 0f);
	}

}
