using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{

    public uint maxHealth;
    public uint currentHealth;
    [SyncVar]
    public uint syncCurrentHealth;

    [SerializeField]
    private bool _multiplayerEnabled;


    void Start()
    {
        syncCurrentHealth = maxHealth;
        currentHealth = maxHealth;
        _multiplayerEnabled = (TanksGameManager.Instance.gameMode == TanksGameManager.GameMode.MULTIPLAYER);
    }


    public void CauseDamage(uint damage)
    {
        if (_multiplayerEnabled && isServer)
        {
            syncCurrentHealth -= damage;
            syncCurrentHealth = (syncCurrentHealth < 0) ? 0 : syncCurrentHealth;
        }
        else
        {
            currentHealth -= damage;
            currentHealth = (currentHealth < 0) ? 0 : currentHealth;
        }
    }


    public void IncreaseHealth(uint addition)
    {
        if (_multiplayerEnabled && isServer)
        {
            syncCurrentHealth += addition;
            syncCurrentHealth = (syncCurrentHealth > maxHealth) ? maxHealth : syncCurrentHealth;
        }
        else
        {
            currentHealth += addition;
            currentHealth = (currentHealth > maxHealth) ? maxHealth : currentHealth;
        }
    }
}