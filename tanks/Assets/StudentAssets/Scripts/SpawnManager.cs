using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnManager : Singleton<SpawnManager>
{
    Quaternion zero = Quaternion.Euler(0, 0, 0);

    private GameObjectPool _objectPool = new GameObjectPool();

	private void Start()
	{
	}

	public GameObject Instantiate(GameObject prefab)
    {
        var objectInstance = _objectPool.CreateObject(prefab);
        objectInstance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        objectInstance.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        return objectInstance;
    }

    public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var objectInstance = _objectPool.CreateObject(prefab);
        objectInstance.transform.position = position;
        objectInstance.transform.rotation = rotation;
        objectInstance.tag = prefab.tag;

        return objectInstance;
    }


    public void Destroy(GameObject prefabInstance)
    {
        prefabInstance.SetActive(false);
        prefabInstance.transform.position = Vector3.zero;
        prefabInstance.transform.rotation = zero;
    }
}
