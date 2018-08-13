using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerInSingleMode : MonoBehaviour {

    public GameObject playerPrefab;

	void Start () {
        var instance = Instantiate(playerPrefab);
	}
}
