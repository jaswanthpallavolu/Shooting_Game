using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons/Melee Weapon")]
public class MeleeWeaponItem : WeaponItem
{
    [Header("WeaponType")]
    public WeaponType weaponType = WeaponType.Melee;
}
