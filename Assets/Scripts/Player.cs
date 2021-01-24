using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead { get { return _isDead; } protected set { _isDead = value; } }

    [SerializeField]
    private int maxHealth = 100;
    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    [SerializeField]
    private GameObject[] disableGameobjectsOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    GameObject deathEffect;
    [SerializeField]
    GameObject SpawnEffect;

    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        setDefaults();
    }

    /*private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpctakeDamage(999);
        }
    }*/

    [ClientRpc]
    public void RpctakeDamage(int _amount)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= _amount;
        Debug.Log(transform.name + " now has " + currentHealth + " health. ");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        for (int i = 0; i < disableGameobjectsOnDeath.Length; i++)
        {
            disableGameobjectsOnDeath[i].SetActive(false);
        }

        Collider _collider = GetComponent<Collider>();
        if (_collider)
        {
            _collider.enabled = false;
        }

        GameObject death = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(death, 5f);

        if (isLocalPlayer)
        {
            GameManager.instance.setSceneCameraState(true);
            GetComponent<PlayerSetup>().playerUI_inst.SetActive(false);
        }

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnDelay);

        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        setDefaults();
    }

    public void setDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        for (int i = 0; i < disableGameobjectsOnDeath.Length; i++)
        {
            disableGameobjectsOnDeath[i].SetActive(true);
        }

        Collider _collider = GetComponent<Collider>();
        if (_collider)
        {
            _collider.enabled = true;
        }

        if (isLocalPlayer)
        {
            GameManager.instance.setSceneCameraState(false);
            GetComponent<PlayerSetup>().playerUI_inst.SetActive(true);
        }

        GameObject spawnFX = (GameObject)Instantiate(SpawnEffect, new Vector3(transform.position.x, 0f, transform.position.z), Quaternion.identity);
        Destroy(spawnFX, 5f);
    }
}
