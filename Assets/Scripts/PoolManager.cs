using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PoolManager : MonoBehaviour {

	Dictionary<int,Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>> ();

	static PoolManager _instance;

	public static PoolManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<PoolManager>();
			}
			return _instance; 
		}
	}

	public void CreatePool(GameObject prefab, int poolSize)
	{
		int poolKey = prefab.GetInstanceID ();

		if (!poolDictionary.ContainsKey(poolKey))
		{
			poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

			for (int i = 0; i < poolSize; i++)
			{
				ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
				poolDictionary[poolKey].Enqueue(newObject);
				newObject.SetParent(prefab.transform.parent);
			}
		}

        //hide the initial prefab used to seed the pool
        prefab.SetActive(false);

	}

	public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		int poolKey = prefab.GetInstanceID();

		if (poolDictionary.ContainsKey(poolKey))
		{
			ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
			poolDictionary[poolKey].Enqueue(objectToReuse);

			objectToReuse.Reuse(position, rotation);
		}

	}

		
	public void ReuseStatusEffectTextObject(GameObject prefab, string text, Vector3 position, Quaternion rotation)
	{
		int poolKey = prefab.GetInstanceID();

		if (poolDictionary.ContainsKey(poolKey))
		{
			ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
			poolDictionary[poolKey].Enqueue(objectToReuse);

			objectToReuse.ReuseStatusEffectText(text, position, rotation);
		}

	}


	public class ObjectInstance
	{
		GameObject gameObject;
		Transform transform;
		TextMeshPro textmeshpro;

		bool hasPoolObjectComponent;
		PoolObject poolObjectScript;

		public ObjectInstance(GameObject objectInstance)
		{
			gameObject = objectInstance;
			transform = gameObject.transform;
            textmeshpro = gameObject.GetComponentInChildren<TextMeshPro>();
			gameObject.SetActive(false);

            if (gameObject.GetComponent<PoolObject>())
            {
                hasPoolObjectComponent = true;
                poolObjectScript = gameObject.GetComponent<PoolObject>();
            }
            else
            {
                Debug.Log("PoolManager: Object does not have PoolObject script attached");
            }
		}

		public void Reuse(Vector3 position, Quaternion rotation)
		{
			if (hasPoolObjectComponent)
			{
				poolObjectScript.OnObjectReuse();
			}

			gameObject.SetActive(true);
			gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;

		}

		public void ReuseStatusEffectText(string text, Vector3 position, Quaternion rotation)
		{
			gameObject.SetActive(true);
			textmeshpro.text = text;
			gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;

			if (hasPoolObjectComponent)
			{
				poolObjectScript.OnObjectReuse();
			}
		}

		public void SetParent(Transform parent)
		{
			transform.SetParent(parent);
		}
	}
}
