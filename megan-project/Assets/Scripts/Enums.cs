using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
}

public enum MovementType
{
    Forward,
    Strafe
}

public enum WeaponModelSlot
{
    LeftHand,
    RightHand
}

public enum WeaponType
{
    Melee,
    Firearm,
    Projectile,
}

public enum WeaponAnimState
{
    FH, // FREE HAND 
    HG, // HANDGUN
    AR, // ASSAULT RIFLE
}