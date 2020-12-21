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

    [Client]
    private void shoot()
    {
        Debug.Log("Shoot");

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
        {
            if (hit.collider.tag == "Player")
            {
                CmdPlayerShotCheck(hit.collider.name, currentWeapon.damage);
            }
        }
    }

    [Command]
    void CmdPlayerShotCheck(string _playerID, int _damage)
    {
        Debug.Log(_playerID + " has been shot");
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpctakeDamage(_damage);
    }
}
