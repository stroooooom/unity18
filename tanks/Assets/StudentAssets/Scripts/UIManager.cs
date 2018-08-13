using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager> {

    [SerializeField]
    private Canvas _canvas;
    private GameObjectPool _UIObjectPool = new GameObjectPool();
    private Dictionary<int, List<GameObject>> _uiObjects = new Dictionary<int, List<GameObject>>();


    public GameObject AddItem(int gameObjectID, GameObject uiObject, bool dontSpawnOnServer)
    {
        if (!_uiObjects.ContainsKey(gameObjectID))
        {
            _uiObjects.Add(gameObjectID, new List<GameObject>());
        }

        var uiObjectInstance = _UIObjectPool.CreateObject(uiObject, dontSpawnOnServer);
        _uiObjects[gameObjectID].Add(uiObjectInstance);
        uiObjectInstance.transform.SetParent(_canvas.transform);

        return uiObjectInstance;
    }

    public void RemoveObjectItems(int gameObjectID)
    {
        if (_uiObjects.ContainsKey(gameObjectID))
        {
            var objectItems = _uiObjects[gameObjectID];
            foreach (var item in objectItems)
            {
                Destroy(item);
            }

            _uiObjects.Remove(gameObjectID);
        }
    }

    public void RemoveAll()
    {

        var keys = _uiObjects.Keys.ToArray();

        foreach (var key in keys)
        {
            RemoveObjectItems(key);
        }

        _uiObjects.Clear();
    }
}
