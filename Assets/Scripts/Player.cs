using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    [SyncVar]
    private int currentHealth;

    private void Awake()
    {
        setDefaults();
    }

    public void takeDamage(int _amount)
    {
        currentHealth -= _amount;
        Debug.Log(transform.name + " now has " + currentHealth + " health. ");
    }

    public void setDefaults()
    {
        currentHealth = maxHealth;
    }
}
