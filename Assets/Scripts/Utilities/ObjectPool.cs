using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	static Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();


	static public void CreatePool(GameObject prefab, int poolSize)
	{
		int poolKey = prefab.GetInstanceID();
		if (!poolDictionary.ContainsKey(poolKey))
		{
			poolDictionary.Add(poolKey, new Queue<GameObject>());

			for (int i = 0; i < poolSize; i++)
			{
				GameObject newObject = Instantiate(prefab) as GameObject;
				newObject.SetActive(false);
				poolDictionary[poolKey].Enqueue(newObject);
			}
		}
	}

	static public GameObject GetObject(GameObject prefab)
	{
		int poolKey = prefab.GetInstanceID();

		if (!poolDictionary.ContainsKey(poolKey))
		{
			Debug.LogError("Unexpected object type");
			return null;
		}

		// Если в конце очереди неактиный объект - можно реюзать его
		if (!poolDictionary[poolKey].Peek().activeSelf)
		{ 
			GameObject objectToReuse = poolDictionary[poolKey].Dequeue();
			poolDictionary[poolKey].Enqueue(objectToReuse);

			objectToReuse.SetActive(true);
			return objectToReuse;
		}
		// Иначе в пуле недостаточно бъектов - добавить новый и использовать его
		else
		{
			GameObject newObject = Instantiate(prefab) as GameObject;
			poolDictionary[poolKey].Enqueue(newObject);
			return newObject;
		}
	}
}