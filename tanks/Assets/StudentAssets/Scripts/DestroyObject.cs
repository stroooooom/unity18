using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestroyObject : NetworkBehaviour
{

    public float time;

    private void OnEnable()
    {
        if (!TanksGameManager.Instance.multiplayerEnabled())
        {
            Invoke("Disable", time);
        }
        else
        {
            if (!isServer)
            {
                return;
            }
            Invoke("Disable", time);
        }

    }

    private void Disable()
    {
        if (TanksGameManager.Instance.multiplayerEnabled() && isClient)
        {
            CmdDisable();
        }
        else if (!TanksGameManager.Instance.multiplayerEnabled())
        {
            SpawnManager.Instance.Destroy(gameObject);
        }
    }


    [Command]
    private void CmdDisable()
    {
        gameObject.SetActive(false);
        RpcDisable();
    }

    [ClientRpc]
    void RpcDisable()
    {
        gameObject.SetActive(false);
    }
}
