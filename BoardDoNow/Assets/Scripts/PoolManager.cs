using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PoolManager
{
	private static Dictionary<GameObject, List<GameObject> > reuseable = new Dictionary<GameObject, List<GameObject> >();
	private static Dictionary<GameObject, GameObject> inUse = new Dictionary<GameObject, GameObject>();
	private static Dictionary<GameObject, int> prefabCount = new Dictionary<GameObject, int>();
	
	
	
	static public GameObject Instantiate(GameObject prefab, int maxCount)
	{
		int count;
		if (prefabCount.TryGetValue(prefab, out count) == false)
			count = 0;
		
		if (count < maxCount)
			return Instantiate(prefab);
		else
			return null;
	}
	static public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		List<GameObject> objs;
		
		if (reuseable.TryGetValue(prefab, out objs))
		{
			foreach (GameObject go in objs)
			{
				if (go != null)
				{
					go.transform.parent = null;
					go.transform.position = position;
					go.transform.rotation = rotation;
					go.SetActive(true);
					objs.Remove(go);
					inUse.Add(go, prefab);
					if (prefabCount.ContainsKey(prefab))
						prefabCount[prefab]++;
					else
						prefabCount.Add(prefab, 1);
					return go;
				}
			}
		}
		GameObject r = GameObject.Instantiate(prefab, position, rotation) as GameObject;
		inUse.Add(r, prefab);
		if (prefabCount.ContainsKey(prefab))
			prefabCount[prefab]++;
		else
			prefabCount.Add(prefab, 1);
		return r;
	}
	static public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, int maxCount)
	{
		int count;
		if (prefab == null)
			return null;
		if (prefabCount.TryGetValue(prefab, out count) == false)
			count = 0;
		
		if (count < maxCount)
		{
			GameObject tmp = PoolManager.Instantiate(prefab, position, rotation);
			return tmp;
		}
		else
			return null;
	}
	static public GameObject Instantiate(GameObject prefab)
	{
		List<GameObject> objs;
		
		if (reuseable.TryGetValue(prefab, out objs))
		{
			foreach (GameObject go in objs)
			{
				if (go != null)
				{
					go.transform.parent = null;
					go.SetActive(true);
					objs.Remove(go);
					inUse.Add(go, prefab);
					if (prefabCount.ContainsKey(prefab))
						prefabCount[prefab]++;
					else
						prefabCount.Add(prefab, 1);
					return go;
				}
			}
		}
		GameObject r = GameObject.Instantiate(prefab) as GameObject;
		inUse.Add(r, prefab);
		if (prefabCount.ContainsKey(prefab))
			prefabCount[prefab]++;
		else
			prefabCount.Add(prefab, 1);
		return r;
	}
	
	
	
	static public void Destroy(GameObject go)
	{
		if (go == null)
			return;
		GameObject prefab;
		
		if (inUse.TryGetValue(go, out prefab))
		{
			List<GameObject> objs;
			
			if (reuseable.TryGetValue(prefab, out objs))
			{
			}
			else
			{
				objs = new List<GameObject>();
				reuseable.Add(prefab, objs);
			}
			objs.Add(go);
			inUse.Remove(go);
			prefabCount[prefab]--;
			go.SetActive(false);
		}
		else
			GameObject.Destroy(go);
	}
	
	
	
	static public void CleanUp()
	{
		reuseable.Clear();
		inUse.Clear();
		prefabCount.Clear();
	}
	
	
	
	static public void PrintDebug()
	{
		int counter = 0;
		foreach (List<GameObject> objs in reuseable.Values)
		{
			counter += objs.Count;
		}
		Debug.Log(inUse.Count + " / " + counter);
	}
}