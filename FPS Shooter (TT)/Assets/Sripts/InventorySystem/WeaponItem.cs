using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class WeaponItem : ScriptableObject
{
    public float damage;
    public string weaponName;
    public int maxAmmoCount;
    public int currentAmmoCount;
}
