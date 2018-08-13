using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityMine : MonoBehaviour {

    [SerializeField]
    private float _detectionRange;
    [SerializeField]
    private float _explosionRange;


	private void OnDrawGizmos()
	{
        Gizmos.color = Color.white;
        Gizmos.matrix = transform.localToWorldMatrix;


        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, _detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRange);

	}


	private void OnDrawGizmosSelected()
	{
        // указывать локальные координаты (?)
        Gizmos.DrawWireCube(transform.position, Vector3.one);
        // для угла обзора игрока
        Gizmos.DrawFrustum(transform.position, 40, 2, 20, .8f);
	}
}
