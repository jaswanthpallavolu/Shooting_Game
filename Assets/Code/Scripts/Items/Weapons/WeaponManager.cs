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

    public void Initiliaze()
    {
        if (weaponType == WeaponType.Firearm)
        {
            FirearmWeaponController weaponController = GetComponent<FirearmWeaponController>();
            // weaponController.InitializeWeapon();
        }
    }

    public void TriggerAction(bool trigger)
    {
        if (weaponType == WeaponType.Firearm)
        {
            FirearmWeaponController weaponController = GetComponent<FirearmWeaponController>();
            weaponController.UpdateLoop(trigger);
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
