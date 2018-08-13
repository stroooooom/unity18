using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;

    public static T Instance
    {
        get{
            if (_instance != null)
            {
                return _instance;
            }

            var instances = FindObjectsOfType<T>();

            if (instances.Length > 1)
            {
                Debug.LogErrorFormat("More than 1 inst of type {0}", typeof(T).Name);
            }

            if (instances.Length == 0)
            {
                var someGO = new GameObject(typeof(T).Name);
                _instance = someGO.AddComponent<T>();
                return _instance;
            }

            _instance = instances[0];
            return _instance;
        }
    }
}
