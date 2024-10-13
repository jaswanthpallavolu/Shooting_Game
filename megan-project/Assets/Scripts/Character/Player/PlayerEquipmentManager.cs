using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager playerManager;
    public WeaponModelInstantiationSlot rightHandSlot;
    public WeaponModelInstantiationSlot leftHandSlot;
    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;
    public WeaponManager rightWeaponManager;
    public WeaponManager leftWeaponManager;

    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
        InitilaizeWeaponSlots();
    }

    void InitilaizeWeaponSlots()
    {
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
            {
                leftHandSlot = weaponSlot;
            }

        }

    }

    protected override void Start()
    {
        base.Start();
        LoadWeaponsOnBothHands();
    }

    public void LoadWeaponsOnBothHands()
    {
        LoadRightWeapon();
        // LoadLeftWeapon();
    }

    public void LoadRightWeapon()
    {
        if (playerManager.playerInventoryManager.currentRightHandWeapon != null)
        {
            string equipAnimation = playerManager.playerInventoryManager.currentRightHandWeapon.equipAnimation;
            WeaponAnimState weaponAnimState = playerManager.playerInventoryManager.currentRightHandWeapon.weaponAnimState;
            SelectWeaponAnimState(weaponAnimState);
            if (!string.IsNullOrEmpty(equipAnimation))
            {
                // [TODO] PLAY UNEQUIP ANIMATION
                playerManager.playerAnimatorManager.PlayTargetAnimation(equipAnimation, false, true, true, true);
                playerManager.playerAnimatorManager.HandleFirearmAnimRig();

            }

            rightHandWeaponModel = Instantiate(playerManager.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.OnLoadWeapon(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetCharacterManager(playerManager);
            rightWeaponManager.SetWeaponDamage(playerManager.playerInventoryManager.currentRightHandWeapon);
        }
    }

    public void SwitchRightHandWeapon(int weaponId)
    {
        if (weaponId == playerManager.playerInventoryManager.rightHandWeaponIndex) return;
        rightHandSlot.UnLoadWeapon();
        playerManager.OnCurrentRighHandWeaponIdChange(
            playerManager.playerInventoryManager.rightHandWeaponIndex,
            weaponId
            );
        playerManager.playerInventoryManager.rightHandWeaponIndex = weaponId;
    }

    public void SelectWeaponAnimState(WeaponAnimState weaponAnimState)
    {
        foreach (var item in animStateList)
        {
            if (item.AnimState == weaponAnimState)
            {
                playerManager.animator.SetBool(item.AnimStateId, true);
            }
            else
            {
                playerManager.animator.SetBool(item.AnimStateId, false);
            }
        }
    }
}
