using UnityEngine;
using System.Collections;

public class StatusEffectTextContainer : MonoBehaviour {

	public GameObject prefab;

	void Start () 
	{
        PoolManager.instance.CreatePool(prefab, 10);
	}

	public void ShowTextEffect(string text, Vector3 position)
	{
		Quaternion rotation = prefab.transform.rotation;
		PoolManager.instance.ReuseStatusEffectTextObject(prefab, text, position, rotation);
	}

}
