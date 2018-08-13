using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITankVisionGizmo : MonoBehaviour {

    [SerializeField]
    private float _sightDist;
    [SerializeField]
    private float _sightHeight;
    [SerializeField]
    private float _viewBoundary;
    [SerializeField]
    private Transform _turret;

    private void OnDrawGizmos()
    {
        _sightDist = GetComponent<AITank>().sightDist;
        _sightHeight = GetComponent<AITank>().sightHeight;
        _turret = GetComponent<AITank>().turret;
        _viewBoundary = GetComponent<AITank>().viewBoundary;

        Gizmos.color = Color.green;

        var _heightVector = _sightHeight * Vector3.up;
        var rayOrigin = _turret.position + _heightVector;
        var rayDirection = _turret.forward * _sightDist;
        var sideOffset = _turret.right * _sightDist / _viewBoundary;

        Gizmos.DrawRay(rayOrigin, rayDirection);
        Gizmos.DrawRay(rayOrigin, rayDirection - sideOffset);
        Gizmos.DrawRay(rayOrigin, rayDirection + sideOffset);
    }
}
