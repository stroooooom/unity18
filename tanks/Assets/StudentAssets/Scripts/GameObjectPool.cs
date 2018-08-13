using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameObjectPool : NetworkBehaviour
{
    private Dictionary<GameObject, List<GameObject>> _instances = new Dictionary<GameObject, List<GameObject>>();

    public GameObject CreateObject(GameObject objectPrefab, bool dontSpawnOnServer = false)
    {
        if (_instances.ContainsKey(objectPrefab))
        {
            var prefabInstances = _instances[objectPrefab];
            foreach (var instance in prefabInstances)
            {
                if (instance.active == false)
                {
                    instance.SetActive(true);
                    return instance;
                }
            }
        }

        return AddNewInstance(objectPrefab, 1, false, dontSpawnOnServer);
    }

    public void Reserve(GameObject objectPrefab, uint count)
    {
        AddNewInstance(objectPrefab, count, true);
    }

    private GameObject AddNewInstance(GameObject objectPrefab, uint count = 1, bool reserve = false, bool dontSpawnOnServer = false)
    {
        objectPrefab.SetActive(false);

        if (!_instances.ContainsKey(objectPrefab))
        {
            _instances.Add(objectPrefab, new List<GameObject>());
        }

        GameObject newPrefabInstance = null;
        var prefabInstances = _instances[objectPrefab];
        for (uint i = 0; i < count; i ++)
        {

            newPrefabInstance = Instantiate(objectPrefab);
            newPrefabInstance.SetActive(false);
            if (TanksGameManager.Instance.multiplayerEnabled() && !dontSpawnOnServer)
            {
                NetworkServer.Spawn(newPrefabInstance);
            }
            prefabInstances.Add(newPrefabInstance);
        }

        if (!reserve && newPrefabInstance)
        {
            newPrefabInstance.SetActive(true);
        }
        else
        {
            newPrefabInstance = null;
        }

        return newPrefabInstance;
    }
}
