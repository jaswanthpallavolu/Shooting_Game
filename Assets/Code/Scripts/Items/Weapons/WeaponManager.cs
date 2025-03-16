using System.Collections;
using System.Collections.Generic;
using Ballistics;
using SG_Project;
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

    public void TriggerAction(bool fullAuto)
    {
        if (weaponType == WeaponType.Firearm)
        {
            FirearmWeaponController weaponController = GetComponent<FirearmWeaponController>();
            if (fullAuto && WeaponMode.FullAuto == weaponController.Mode ||
                !fullAuto && (WeaponMode.SingleShot == weaponController.Mode || WeaponMode.FullAuto == weaponController.Mode))
                weaponController.WeaponShoot();
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
