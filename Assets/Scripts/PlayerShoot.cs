using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private playerWeapon weapon;
    [SerializeField]
    private GameObject weaponGFX;
    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    Camera cam;

    [SerializeField]
    LayerMask mask;

    private void Start()
    {
        if (cam == null)
        {
            Debug.Log("No camera reference");
            this.enabled = false;
        }
        weaponGFX.layer = LayerMask.NameToLayer(weaponLayerName);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shoot();
        }
    }

    [Client]
    private void shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            if (hit.collider.tag == "Player")
            {
                CmdPlayerShotCheck(hit.collider.name, weapon.damage);
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
