using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private string weaponLayerName = "Weapon";
    [SerializeField]
    Transform weaponHolder;
    [SerializeField]
    playerWeapon primaryWeapon;
    playerWeapon currentWeapon;

    private void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public playerWeapon getCurrentWeapon { get { return currentWeapon; } }

    void EquipWeapon(playerWeapon _weapon)
    {
        currentWeapon = _weapon;
        GameObject _weaponInst = Instantiate(_weapon.weaponObject, weaponHolder.position, weaponHolder.rotation) as GameObject;
        _weaponInst.transform.SetParent(weaponHolder);

        if (isLocalPlayer)
        {
            _weaponInst.layer = LayerMask.NameToLayer(weaponLayerName);
        }
    }
}
