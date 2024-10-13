using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] WeaponType weaponType;
    [SerializeField] DamageCollider damageCollider;
    [SerializeField] WeaponDamage weaponDamage;

    void Awake()
    {
        if (weaponType == WeaponType.Firearm)
        {
            weaponDamage = GetComponentInChildren<WeaponDamage>();
        }
        else if (weaponType == WeaponType.Melee)
        {
            damageCollider = GetComponentInChildren<DamageCollider>();
        }
    }

    public void SetCharacterManager(CharacterManager characterManager)
    {
        if (weaponType == WeaponType.Firearm)
        {
            weaponDamage.CharacterCausingDamage = characterManager;
        }
        else if (weaponType == WeaponType.Melee)
        {
            damageCollider.CharacterCausingDamage = characterManager;
        }
    }

    public void SetWeaponDamage(WeaponItem weapon)
    {
        // meleeWeaponDamageCollider.physicalDamage = weapon.physicalDamage;
    }
}
