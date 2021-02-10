using UnityEngine;

[System.Serializable]
public class playerWeapon
{
    public string name = "Glock-18";
    public int damage = 10;
    public float range = 100f;
    public GameObject weaponObject;
    public float fireRate = 0f;
    public float reloadTime = 1f;
    public int maxBullets = 20;
    [HideInInspector]
    public int currentBullets;

    public playerWeapon()
    {
        currentBullets = maxBullets;
    }
}
