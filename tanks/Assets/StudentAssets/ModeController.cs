using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ModeController : NetworkBehaviour {

    public bool _multiplayerMode = false;

	public override void OnStartClient()
	{
        _multiplayerMode = true;
	}
}
