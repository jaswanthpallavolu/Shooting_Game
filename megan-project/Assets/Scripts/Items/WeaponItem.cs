using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item
{
    // ANIMATOR CONTROLLER OVERRIDE
    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Damage")]
    public float physicalDamage;

    // WEAPON MODIFIER
    [Header("Attack Modifiers")]
    public float light_attack_01_modifier = 1f;
    // LIGHT ATTACK 
    // HEAVY ATTACK
    // CRITICAL DAMAGE

    [Header("Stamina Cost Modifiers")]
    public int baseStaminaCost;
    public float lightAttackStaminaCostMultiplier = 1f;
    // Various STAMINA COSTS [LIGHT ATTACK, HEAVY ATTACK, BLOCKING]

    // ITEM BASED ACTIONS (RB, RT, LB, LT)
    [Header("Actions")]
    public WeaponItemAction oh_RB_Action;
    public WeaponItemAction oh_RT_Action;
}
