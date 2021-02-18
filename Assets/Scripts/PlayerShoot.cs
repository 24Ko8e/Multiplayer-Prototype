using System;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    Camera cam;

    [SerializeField]
    LayerMask mask;

    playerWeapon currentWeapon;
    WeaponManager weaponManager;

    private void Start()
    {
        if (cam == null)
        {
            Debug.Log("No camera reference");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();
    }

    float _timer = 0f;
    private void Update()
    {
        currentWeapon = weaponManager.getCurrentWeapon;

        _timer -= Time.deltaTime;

        if (PauseMenu.isPaused)
            return;

        if (currentWeapon.currentBullets < currentWeapon.maxBullets)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                weaponManager.Reload();
                return;
            }
        }

        if (currentWeapon.fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1"))
            {
                if (_timer <= 0)
                {
                    shoot();
                    _timer = 1f / currentWeapon.fireRate;
                }
            }
        }
    }

    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    [ClientRpc]
    void RpcDoShootEffect()
    {
        if (weaponManager.getCurrentGraphics.muzzleFlash.isPlaying)
            return;

        weaponManager.getCurrentGraphics.muzzleFlash.Play();
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal, string _name)
    {

        if (_name.StartsWith("Floor"))
        {
            GameObject hitEffect = (GameObject)Instantiate(weaponManager.getCurrentGraphics.hitEffectPrefabs[0], _pos, Quaternion.LookRotation(_normal));
            Destroy(hitEffect, 5f);
        }
        else
        {
            GameObject hitEffect = (GameObject)Instantiate(weaponManager.getCurrentGraphics.hitEffectPrefabs[1], _pos, Quaternion.LookRotation(_normal));
            Destroy(hitEffect, 5f);
        }
    }

    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal, string _name)
    {
        RpcDoHitEffect(_pos, _normal, _name);
    }

    [Client]
    private void shoot()
    {
        if (!isLocalPlayer || weaponManager.isReloading)
        {
            return;
        }

        if (currentWeapon.currentBullets <= 0)
        {
            weaponManager.Reload();
            return;
        }
        currentWeapon.currentBullets--;

        CmdOnShoot();

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
        {
            if (hit.collider.tag == "Player")
            {
                CmdPlayerShotCheck(hit.collider.name, currentWeapon.damage, transform.name);
            }

            CmdOnHit(hit.point, hit.normal, hit.transform.name);
        }

        if (currentWeapon.currentBullets <= 0)
        {
            weaponManager.Reload();
        }
    }

    [Command]
    void CmdPlayerShotCheck(string _playerID, int _damage, string _sourcePlayerID)
    {
        Debug.Log(_playerID + " has been shot");
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpctakeDamage(_damage, _sourcePlayerID);
    }
}
