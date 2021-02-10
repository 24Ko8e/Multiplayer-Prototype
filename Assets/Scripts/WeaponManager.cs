using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private string weaponLayerName = "Weapon";
    [SerializeField]
    Transform weaponHolder;
    [SerializeField]
    playerWeapon primaryWeapon;
    playerWeapon currentWeapon;
    WeaponGraphics currentGraphics;

    public bool isReloading = false;

    private void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public playerWeapon getCurrentWeapon { get { return currentWeapon; } }
    public WeaponGraphics getCurrentGraphics { get { return currentGraphics; } }

    void EquipWeapon(playerWeapon _weapon)
    {
        currentWeapon = _weapon;
        GameObject _weaponInst = Instantiate(_weapon.weaponObject, weaponHolder.position, weaponHolder.rotation) as GameObject;
        _weaponInst.transform.SetParent(weaponHolder);
        
        currentGraphics = _weaponInst.GetComponent<WeaponGraphics>();

        if (currentGraphics == null)
            Debug.LogError("No WeaponGraphics component on the weapon object: " + _weaponInst.name);

        if (isLocalPlayer)
        {
            Util.setLayerRecursively(_weaponInst, LayerMask.NameToLayer(weaponLayerName));
        }
    }

    public void Reload()
    {
        if (isReloading)
        {
            return;
        }

        StartCoroutine(IEreload());
    }

    IEnumerator IEreload()
    {
        isReloading = true;
        CmdOnReload();

        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentWeapon.currentBullets = currentWeapon.maxBullets;

        isReloading = false;
    }

    [Command]
    void CmdOnReload()
    {
        RpcOnReload();
    }

    [ClientRpc]
    private void RpcOnReload()
    {
        Animator anim = currentGraphics.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Reload");
        }
    }
}
