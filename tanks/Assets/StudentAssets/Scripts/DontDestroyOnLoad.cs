using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DontDestroyOnLoad : NetworkBehaviour {

	private void Awake()
	{
        DontDestroyOnLoad(gameObject);
	}
}
