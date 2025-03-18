using System.Collections;
using System.Collections.Generic;
using Ballistics;
using SG_Project;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] WeaponType weaponType;
    private DamageCollider damageCollider;
    private FirearmWeaponController weaponController;

    void Awake()
    {
        if (weaponType == WeaponType.Firearm)
        {
            weaponController = GetComponent<FirearmWeaponController>();
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
            if (fullAuto && WeaponMode.FullAuto == weaponController.Mode ||
                !fullAuto && (WeaponMode.SingleShot == weaponController.Mode || WeaponMode.FullAuto == weaponController.Mode))
                weaponController.WeaponShoot();
        }
    }

    public void SetCharacterManager(CharacterManager characterManager)
    {
        if (weaponType == WeaponType.Firearm)
        {
            weaponController.Character = characterManager;
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
