using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HealthBar : NetworkBehaviour
{
    public GameObject healthBarPrefab;

    private Camera _camera;
    private Health _health;

    private RectTransform _healthBarTransform;
    private RectTransform _healthBarForegroundTransform;

    private float _defaultXscale;
    private float _defaultYscale;
    private float _defaultZscale;

    const float offsetY = 20;

	private void Start()
	{
        _health = GetComponent<Health>();
        var _healthBarInstance = UIManager.Instance.AddItem(gameObject.GetInstanceID(), healthBarPrefab, true);

        _healthBarTransform = _healthBarInstance.GetComponent<RectTransform>();
        _healthBarForegroundTransform = (UnityEngine.RectTransform)_healthBarTransform.GetChild(2);

        _defaultXscale = _healthBarForegroundTransform.localScale.x;
        _defaultYscale = _healthBarForegroundTransform.localScale.y;
        _defaultZscale = _healthBarForegroundTransform.localScale.z;
	}

	void Update ()
    {
        var multiplayerEnabled = TanksGameManager.Instance.multiplayerEnabled();

        if (_healthBarTransform == null || _healthBarForegroundTransform == null)
        {
            return;
        }


        if (!multiplayerEnabled)
        {
            _camera = Camera.main;
        }
        else
        {
            var ClientCamera = GameObject.FindWithTag("ClientCamera");
            if (ClientCamera == null)
            {
                return;
            }
            _camera = ClientCamera.GetComponent<Camera>();
        }

        if (_camera == null)
        {
            return;
        }

        var screenPosition = _camera.WorldToScreenPoint(transform.position);
        screenPosition.y = screenPosition.y + offsetY;
        _healthBarTransform.position = screenPosition;

        float ratio;
        if (multiplayerEnabled)
        {
            ratio = (float)_health.syncCurrentHealth / (float)_health.maxHealth;
        }
        else
        {
            ratio = (float)_health.currentHealth / (float)_health.maxHealth;
        }

        if (ratio < 1e-5)
        {
            if (isLocalPlayer)
            {
                UIManager.Instance.RemoveAll();
                //RemoveItems();
            }
            else
            {
                RemoveItems();
            }
        }

        _healthBarForegroundTransform.localScale = new Vector3(ratio * _defaultXscale, _defaultYscale, _defaultZscale);
	}



    private void RemoveItems()
    {
        UIManager.Instance.RemoveObjectItems(gameObject.GetInstanceID());
    }


    [ClientRpc]
    void RpcDestroyObjectItems(int instanceID)
    {
        DestroyItemsOfPlayer(instanceID);
    }


	void DestroyItemsOfPlayer(int instanceID)
    {
        UIManager.Instance.RemoveObjectItems(instanceID);
        if (GetInstanceID() == instanceID)
        {
            UIManager.Instance.RemoveAll();
        }
    }
}
