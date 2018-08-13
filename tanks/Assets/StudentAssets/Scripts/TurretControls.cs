using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurretControls : NetworkBehaviour {
    
    public Transform Turret;
    public float turretTurnSpeed;

    [SerializeField]
    private Camera _camera;

    // current mode
    private bool _multiplayerEnabled;

    // Use this for initialization
    void Start()
    {
        _multiplayerEnabled = (TanksGameManager.Instance.gameMode == TanksGameManager.GameMode.MULTIPLAYER);

        if (!_multiplayerEnabled)
        {
            gameObject.GetComponent<CameraController>().enabled = false;
        }
    }


    void Update()
    {
        if (_multiplayerEnabled && !isLocalPlayer)
        {
            return;
        }

        Turn();
    }

    void Turn()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000);
        Vector3 mousePosition = hit.point;
        mousePosition.y = Turret.position.y;

        var rotation = Quaternion.LookRotation(mousePosition - Turret.position);
        Turret.rotation = Quaternion.Slerp(Turret.rotation, rotation, Time.deltaTime * turretTurnSpeed);
    }
}
