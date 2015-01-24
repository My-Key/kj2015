using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour {

    public GameObject prefabToSpawn;
	void Start () 
    {
        GameObject obj = PoolManager.Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        obj.transform.parent = transform;
        obj.transform.localScale = new Vector3(1,1,1);
	}

}
