using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSpeedUpBonus : MonoBehaviour {

    public float bonusSpeed;
    public float time = 2f;

    private TankControls _tankControls;
    private float _defaultSpeed;
    private float _currentSpeed;


    void Start()
    {
        _tankControls = GetComponent<TankControls>();
        _defaultSpeed = _tankControls.Speed;

        _currentSpeed = _defaultSpeed;
    }

    void AddBonusSpeed()
    {
        if (System.Math.Abs(_defaultSpeed - _tankControls.Speed) < 1e-3)
        {
            _tankControls.Speed += bonusSpeed;
            CancelInvoke("RestoreSpeed");
            Invoke("RestoreSpeed", time);

            _currentSpeed += bonusSpeed;
        }
    }

    void RestoreSpeed()
    {
        if (System.Math.Abs(_defaultSpeed - _tankControls.Speed) > 1e-3)
        {
            _tankControls.Speed = _defaultSpeed;

            _currentSpeed = _defaultSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SpeedPill(Clone)")
        {
            AddBonusSpeed();
        }

        Destroy(other.gameObject, 0);
    }
}
