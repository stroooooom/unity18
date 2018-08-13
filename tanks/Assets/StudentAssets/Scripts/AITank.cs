using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITank : MonoBehaviour {

    const float timeBetweenChangingState = 2f;

    public enum State
    {
        PATROL,
        CHASE,
        WATCH
    }

    public GameObject patrolPositions;

    public Transform turret;
    public Transform barrel;
    public Rigidbody shellPrefab;

    public float sightDist = 40f;
    public float sightHeight = 0.5f;
    [Range(5f, 1f)]
    public float viewBoundary = 2;
    public float shotInterval = 0.3f;
    public float shotForce = 2000f;

    public int startPatrolPosID = 1;
    public float chaseTime;
    public float holdingDistance;
    public float additionalSpeed;

    private float _defaultSpeed;

    private NavMeshAgent _tank;
    private Transform[] _patrolPoints;
    private int _currentPatrolPosID = 1;

    private Health _health;
    private Vector3 _heightVector;
    private int _lastShellID;
    private int _AITankInstanceId;

    [SerializeField] private State _state;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _currentChasingTime;
    [SerializeField] private int _currentWatchStep;

    private bool _stateRecentlyChanged;
    private bool _disabled;


	void Start () {
        _tank = GetComponent<NavMeshAgent>();
        _tank.autoRepath = true; // need it?

        _state = State.PATROL;
        _patrolPoints = patrolPositions.GetComponentsInChildren<Transform>();
        _currentPatrolPosID = startPatrolPosID;

        _health = GetComponent<Health>();
        _heightVector = Vector3.up * sightHeight;
        _AITankInstanceId = gameObject.GetInstanceID();
        _lastShellID = _AITankInstanceId;
        _lastShellID = _AITankInstanceId;
        _currentChasingTime = 0;

        _stateRecentlyChanged = false;
        _disabled = false;
        _target = null;
	}


    private void FixedUpdate()
	{
        if (_disabled)
        {
            return;
        }

        switch (_state)
        {
            case State.PATROL:
                Patrol();
                break;
            case State.CHASE:
                Chase();
                break;
            case State.WATCH:
                Watch();
                break;
        }
    }


    private void ChangeState(State state)
    {
        if (_stateRecentlyChanged)
        {
            return;
        }

        StopAllCoroutines();
        _tank.isStopped = false;

        switch (state)
        {
            case State.CHASE:
                _tank.speed += additionalSpeed;
                _tank.destination = _target.transform.position;
                StartCoroutine(Shooting());
                break;

            case State.PATROL:
                _target = null;
                _currentChasingTime = 0;
                _tank.speed = _defaultSpeed;
                _tank.destination = _patrolPoints[_currentPatrolPosID].position;
                turret.LookAt(_tank.destination);
                break;

            case State.WATCH:
                _target = null;
                _tank.isStopped = true;
                _currentWatchStep = 0;
                break;
        }

        _state = state;
        _stateRecentlyChanged = true;
        Invoke("ResetStateChange", timeBetweenChangingState);
    }

    private void ResetStateChange()
    {
        _stateRecentlyChanged = false;
    }


    // CHECK FOR ENEMIES
    private void DetectEnemy()
    {
        var rayOrigin = turret.position + _heightVector;
        var rayDirection = turret.forward * sightDist;
        var sideOffset = turret.right * sightDist / viewBoundary;

        if (DetectEnemyOnRay(rayOrigin, rayDirection)) { return; }
        if (DetectEnemyOnRay(rayOrigin, rayDirection - sideOffset)) { return; }
        if (DetectEnemyOnRay(rayOrigin, rayDirection + sideOffset)) { return; }
    }

    private bool DetectEnemyOnRay(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;
        Physics.Raycast(origin, direction, out hit, sightDist);
        if (hit.collider == null)
        {
            return false;
        }

        var collidedObject = hit.collider.gameObject;
        if (collidedObject.tag == "Player")
        {
            _target = collidedObject;
            ChangeState(State.CHASE);
            return true;
        }

        return false;
    }


    // STATE: PATROL
    void Patrol()
    {
        if (_tank.remainingDistance < 1)
        {
            GoToNextPatrolPoint();
        }

        turret.LookAt(_tank.destination);
        DetectEnemy();
    }

    private void GoToNextPatrolPoint()
    {
        _currentPatrolPosID = (_currentPatrolPosID >= _patrolPoints.Length - 1) ? 1 : _currentPatrolPosID + 1;
        _tank.destination = _patrolPoints[_currentPatrolPosID].position;
    }


    // STATE: WATCH
    void Watch()
    {
        Vector3 lookDirection = Vector3.zero;

        switch (_currentWatchStep)
        {
            case 0:
                lookDirection = _tank.transform.right * -1;
                break;
            case 1:
                lookDirection = _tank.transform.forward * -1;
                break;
            case 2:
                lookDirection = _tank.transform.right;
                break;
            case 3:
                lookDirection = _tank.transform.forward;
                break;
            case 4:
                lookDirection = _tank.transform.forward;
                break;
        }

        if (_currentWatchStep > 4)
        {
            ChangeState(State.PATROL);
        }

        if (Mathf.Abs(Vector3.SignedAngle(turret.forward, lookDirection, Vector3.up)) < 10)
        {
            _currentWatchStep++;
        }

        var rotation = Quaternion.LookRotation(lookDirection);
        turret.rotation = Quaternion.Slerp(turret.rotation, rotation, (float) Time.smoothDeltaTime * 2);
        DetectEnemy();
    }


    // CHASING TARGET
    void Chase()
    {
        _currentChasingTime += Time.fixedDeltaTime;
        var distanceToTarget = Vector3.Distance(_tank.transform.position, _target.transform.position);

        if ((chaseTime > 0 && _currentChasingTime > chaseTime) || (distanceToTarget > sightDist))
        {
            ChangeState(State.PATROL);
            return;
        }

        var targetPos = _target.transform.position;
        targetPos.y = turret.position.y;
        turret.LookAt(targetPos);

        if (distanceToTarget > holdingDistance && distanceToTarget <= sightDist)
        {
            _tank.destination = targetPos;
        }
    }


    IEnumerator Shooting()
    {
        while (true)
        {
            // check target's visibility
            var direction = _target.transform.position - turret.position;

            RaycastHit hit;
            Physics.Raycast(turret.position, direction, out hit);
            if (hit.collider.gameObject.tag != "Player")
            {
                // go around the obstacle
                _tank.destination = _target.transform.position;
            }
            else
            {
                Shot();
            }

            yield return new WaitForSeconds(shotInterval);
        }
    }


    void Shot()
    {
        var shell = SpawnManager.Instance.Instantiate(shellPrefab.gameObject, barrel.position, barrel.rotation);
        shell.GetComponent<Rigidbody>().AddForce(shell.transform.forward * shotForce);
    }




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Shell(Clone)" || collision.gameObject.tag != "Player")
        {
            return;
        }

        if (_state == State.PATROL)
        {
            ChangeState(State.WATCH);
            return;
        }

        var currentShellID = collision.gameObject.GetInstanceID();
        if (currentShellID == _lastShellID && currentShellID != _AITankInstanceId)
        {
            return;
        }
        _lastShellID = currentShellID;

        _health.CauseDamage(1);
        if (_health.currentHealth == 0)
        {
            StopAllCoroutines();

            _tank.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
            _tank.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            _tank.GetComponent<NavMeshAgent>().enabled = false;

            gameObject.GetComponent<TankDeath>().Death();
            UIManager.Instance.RemoveObjectItems(gameObject.GetInstanceID());
            Destroy(gameObject, 8);
            _disabled = true;
        }
    }
}
