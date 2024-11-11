using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    PlayerManager player;
    [SerializeField] Transform HGRig;
    [SerializeField] Transform HGRigAimTarget;
    [SerializeField] Transform ARRig;
    [SerializeField] Transform ARRigAimTarget;
    List<AnimRigList> animRigList;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<PlayerManager>();
        animRigList = new List<AnimRigList>(){
            new AnimRigList {AnimState = WeaponAnimState.HG, Rig = HGRig,AimTarget = HGRigAimTarget },
            new AnimRigList {AnimState = WeaponAnimState.AR, Rig = ARRig, AimTarget = ARRigAimTarget},
        };
    }

    protected override void Update()
    {
        base.Update();
        // HandleFirearmAnimRig();
        SetCurrentRigWeight(player.isAiming ? 1 : 0);
    }

    private void OnAnimatorMove()
    {
        if (player.applyRootMotion)
        {
            Vector3 velocity = player.animator.deltaPosition;
            if (player.characterController.enabled) player.characterController.Move(velocity);
            else player.transform.position += velocity;
            if (!player.isAiming) player.transform.rotation *= player.animator.deltaRotation;
        }
    }

    public void EnableController()
    {
        player.characterController.enabled = true;
    }

    // HANDLE RIG
    public void HandleFirearmAnimRig()
    {
        WeaponAnimState weaponAnimState = player.playerInventoryManager.currentRightHandWeapon.weaponAnimState;
        SelectWeaponAnimRig(weaponAnimState);

    }

    void SelectWeaponAnimRig(WeaponAnimState animState)
    {
        foreach (var animRig in animRigList)
        {
            animRig.Rig.GetComponent<Rig>().weight = 0;
            if (animRig.AnimState == animState)
            {
                currentRig = animRig.Rig;
                currentRigAimTarget = animRig.AimTarget;
                // animRig.Rig.GetComponent<Rig>().weight = 1;
            }
        }
    }

    public void SetCurrentRigWeight(float weight)
    {
        if (currentRig) currentRig.GetComponent<Rig>().weight = weight;
    }

}

class AnimRigList
{
    public WeaponAnimState AnimState { get; set; }
    public Transform Rig { get; set; }
    public Transform AimTarget { get; set; }
}
