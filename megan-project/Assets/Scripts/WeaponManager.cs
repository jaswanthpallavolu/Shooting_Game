using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // [SerializeField] WeaponType weaponType;
    // [SerializeField] MeleeWeaponDamageCollider meleeWeaponDamageCollider;

    void Awake()
    {
        // meleeWeaponDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(WeaponItem weapon)
    {
        // meleeWeaponDamageCollider.physicalDamage = weapon.physicalDamage;
    }
}
