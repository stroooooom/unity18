using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TankControls : NetworkBehaviour
{
    public GameObject ShellPrefab;
	public Transform Barrel;

	public float Speed;
	public float TurnSpeed;
    public float shotForce = 3000;

	private float _forwardAxis;
	private float _sideAxis;

	private Rigidbody _rigidbody;
    private Health _health;

    private bool _disabled;

	void Start ()
	{
		_rigidbody = GetComponent<Rigidbody>();
        _health = GetComponent<Health>();

        _disabled = false;
	}


	private void FixedUpdate()
	{
        if (_disabled)
        {
            return;
        }
        
        if (!isLocalPlayer && TanksGameManager.Instance.multiplayerEnabled())
        {
            return;
        }

		Move();
		Turn();
	}


	void Update ()
	{
        if (_disabled)
        {
            return;
        }

        if (!isLocalPlayer && TanksGameManager.Instance.multiplayerEnabled())
        {
            return;
        }

		_forwardAxis = Input.GetAxis("Vertical1");
		_sideAxis = Input.GetAxis("Horizontal1");
		
		if (Input.GetMouseButtonDown(0))
		{
            if (TanksGameManager.Instance.multiplayerEnabled())
            {
                CmdShot(netId.Value);
            }
            else if (!TanksGameManager.Instance.multiplayerEnabled())
            {
                Shot();
            }
		}
	}


    void Move()
    {
        var shift = Speed * transform.forward * Time.deltaTime * _forwardAxis;
        _rigidbody.MovePosition(_rigidbody.position + shift);
    }


    void Turn()
    {
        var turn = TurnSpeed * Time.deltaTime * _sideAxis;
        var turnY = Quaternion.Euler(0, turn, 0);

        _rigidbody.MoveRotation(_rigidbody.rotation * turnY);
    }



    /// singleplayer mode
    private void Shot()
    {
        var shell = SpawnManager.Instance.Instantiate(ShellPrefab, Barrel.position, Barrel.rotation);
        shell.GetComponent<Rigidbody>().AddForce(Barrel.transform.forward * shotForce);
        shell.tag = "Player";
    }


    /// multiplayer mode
    [Command]
    private void CmdShot(uint netIdValue)
    {
        var shell = SpawnManager.Instance.Instantiate(ShellPrefab, Barrel.position, Barrel.rotation);
        shell.GetComponent<Rigidbody>().AddForce(Barrel.transform.forward * shotForce);
        shell.GetComponent<OwnerID>().ownerID = netIdValue;
        shell.SetActive(true);

        RpcShot(shell);
    }


    [ClientRpc]
    private void RpcShot(GameObject shell)
    {
        shell.SetActive(true);
    }
    //




	private void OnCollisionEnter(Collision collision)
	{
        if (TanksGameManager.Instance.multiplayerEnabled() && !isLocalPlayer)
        {
            return;
        }

        if (collision.gameObject.name != "Shell(Clone)")
        {
            return;
        }

        var multiplayerEnabled = TanksGameManager.Instance.multiplayerEnabled();
        if ((!multiplayerEnabled && collision.gameObject.tag == "Player") || 
            (multiplayerEnabled && collision.gameObject.GetComponent<OwnerID>().ownerID == netId.Value))
        {
            return;
        }

        if (multiplayerEnabled)
        {
            CmdCauseDamage(1, netId.Value);
        }
        else
        {
            _health.CauseDamage(1);
            if (_health.currentHealth == 0)
            {
                gameObject.GetComponent<TankDeath>().Death();
                UIManager.Instance.RemoveObjectItems(gameObject.GetInstanceID());
                Destroy(gameObject, 8);
                _disabled = true;
                _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            }
        }
    }


    [Command]
    void CmdCauseDamage(uint damage, uint objectNetId)
    {
        Debug.LogFormat("objectNetId / netId.Value = {0} / {1}", objectNetId, netId.Value);
             
        if (netId.Value == objectNetId)
        {
            Debug.LogFormat("netId.Value == objectNetId (= {0})", objectNetId);
            _health.CauseDamage(1);
        }

        if (_health.syncCurrentHealth == 0)
        {
            Destroy(gameObject, 8);
            _disabled = true;

            RpcDeath();
        }
    }


    [ClientRpc]
    void RpcDeath()
    {
        gameObject.GetComponent<TankDeath>().Death();
        Destroy(gameObject, 8);
        _disabled = true;
    }
}
