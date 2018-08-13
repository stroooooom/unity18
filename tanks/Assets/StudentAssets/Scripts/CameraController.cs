using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour
{
    [SerializeField]
    private Camera _camera;

	private void Start()
	{
        if (!isLocalPlayer)
        {
            TurnOffCamera();
        }
	}

	private void OnEnable()
	{
        if (!TanksGameManager.Instance.multiplayerEnabled())
        {
            _camera.gameObject.tag = "MainCamera";
        }
    }

    private void TurnOffCamera()
    {
        _camera.enabled = false;
        _camera.gameObject.tag = "Untagged";
    }
}
