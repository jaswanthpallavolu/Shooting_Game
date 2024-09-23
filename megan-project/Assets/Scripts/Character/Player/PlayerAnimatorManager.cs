using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    PlayerManager player;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<PlayerManager>();
    }

    private void OnAnimatorMove()
    {
        if (player.applyRootMotion)
        {
            Vector3 velocity = player.animator.deltaPosition;
            if (player.characterController.enabled) player.characterController.Move(velocity);
            else player.transform.position += velocity;
            player.transform.rotation *= player.animator.deltaRotation;
        }
    }

    public void EnableCharacterController()
    {
        // player.characterController.excludeLayers = new LayerMask();
    }
}
