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

public enum WeaponMode
{
    [Tooltip("A single bullet is fired each shot. The trigger has to be released after each shot.")]
    SingleShot,
    [Tooltip("Each shot fires multiple bullet fragments. The trigger has to be released after each shot.")]
    Shotgun,
    [Tooltip("A single bullet is fired each shot. Holding the trigger will continuously fire bullets.")]
    FullAuto,
    [Tooltip("A single bullet is fired each shot. Holding the trigger will fire x bullets without having to release the trigger.")]
    Burst
}