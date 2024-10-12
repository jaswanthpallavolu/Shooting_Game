using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager characterManager;
    int vertical, horizontal;

    [SerializeField] float parkourAnimationDamp = .2f;
    [SerializeField][Range(0, 1)] public float movementAnimationDamp = .2f;

    protected virtual void Awake()
    {
        characterManager = GetComponentInParent<CharacterManager>();
        horizontal = Animator.StringToHash("horizontal");
        vertical = Animator.StringToHash("vertical");
    }

    protected virtual void Update()
    {

    }

    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, float moveAmount)
    {
        float horizontalAmount = horizontalValue;
        float verticalAmount = verticalValue;
        if (moveAmount <= 0)
        {
            horizontalAmount = 0;
            verticalAmount = 0;
        }
        else if (characterManager.isSprinting)
        {
            verticalAmount = 2f;
        }
        else if (characterManager.isAiming)
        {
            horizontalAmount = Mathf.Clamp(horizontalAmount, -.5f, .5f);
            verticalAmount = Mathf.Clamp(verticalAmount, -.5f, .5f);
        }

        characterManager.animator.SetFloat(horizontal, horizontalAmount, movementAnimationDamp, Time.deltaTime);
        characterManager.animator.SetFloat(vertical, verticalAmount, movementAnimationDamp, Time.deltaTime);

    }

    public void PlayTargetParkourAnimation(string targetAnimation, bool isPerformingAction,
        bool applyRootMotion = true, bool canMove = false, bool canRotate = false)
    {
        characterManager.animator.SetBool("parkour", true);
        characterManager.animator.CrossFade(targetAnimation, parkourAnimationDamp);
        characterManager.isPerformingAction = isPerformingAction;
        characterManager.isPerformingParkourAction = isPerformingAction;
        characterManager.canMove = canMove;
        characterManager.canRotate = canRotate;
        characterManager.applyRootMotion = applyRootMotion;
    }

    public void PlayTargetAnimation(string targetAnimation, bool isPerformingAction,
        bool applyRootMotion = true, bool canMove = false, bool canRotate = false)
    {
        characterManager.animator.CrossFade(targetAnimation, movementAnimationDamp);
        characterManager.isPerformingAction = isPerformingAction;
        characterManager.canMove = canMove;
        characterManager.canRotate = canRotate;
        characterManager.applyRootMotion = applyRootMotion;
    }
}
