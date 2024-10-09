using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    public WeaponItem currentLeftHandWeapon;
    public WeaponItem currentRightHandWeapon;

    public int leftHandWeaponIndex = 0;
    public WeaponItem[] leftHandWeaponSlots = new WeaponItem[3];
    public int rightHandWeaponIndex = 0;
    public WeaponItem[] rightHandWeaponSlots = new WeaponItem[3];

}
