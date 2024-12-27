using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons/Firearm Weapon")]
public class FirearmWeaponItem : WeaponItem
{
    [Header("WeaponType")]
    public WeaponType weaponType = WeaponType.Firearm;
}
