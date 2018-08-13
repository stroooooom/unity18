using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetNetworkEnabled : NetworkBehaviour {

	private void Awake()
	{
        if (isClient || isServer)
        {
            TanksGameManager.Instance.gameMode = TanksGameManager.GameMode.MULTIPLAYER;
            Debug.Log("MULTIPLAYER!");
        }
	}
}
