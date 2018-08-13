using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPillSpawn : MonoBehaviour {

    public GameObject SpeedPillPrefab;

    public float xMin, xMax;
    public float zMin, zMax;
    public float y;
    public float timeInterval;

    private Quaternion zero;


    void Start () {
        zero = new Quaternion { x = 0, y = 0, z = 0, w = 0 };
        InvokeRepeating("SpawnSpeedPill", 0, timeInterval);
	}

    void SpawnSpeedPill()
    {
        float randomX = Random.Range(xMin, xMax);
        float randomZ = Random.Range(zMin, zMax);

        Vector3 pos = new Vector3 {x = randomX, y = y, z = randomZ };

        pos.Set(randomX, y, randomZ);
        Instantiate(SpeedPillPrefab, pos, zero);
    }
}
