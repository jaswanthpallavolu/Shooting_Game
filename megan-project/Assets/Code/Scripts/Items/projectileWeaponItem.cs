using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons/Projectile Weapon")]
public class ProjectileWeaponItem : WeaponItem
{
    [Header("WeaponType")]
    public WeaponType weaponType = WeaponType.Projectile;
}
