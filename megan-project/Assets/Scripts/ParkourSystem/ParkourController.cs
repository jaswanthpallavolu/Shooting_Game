using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    PlayerManager player;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] List<ParkourAction> parkourActions;
    [SerializeField] ParkourAction JumpDownAction;
    [SerializeField] int parkourAnimationLayer = 2;
    [SerializeField] public ObstacleHitData hitData;

    [Header("Debug")]
    [SerializeField] ParkourAction currentAction;
    [SerializeField] GameObject debugSphere;


    void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    void Update()
    {
        hitData = player.environmentScanner.ObstacleCheck();

        if (currentAction != null && player.animator.GetCurrentAnimatorStateInfo(parkourAnimationLayer).IsName(currentAction.animation))
        {
            if (currentAction.rotateToObstacle)
            {
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, currentAction.targetRotation, currentAction.rotationSpeed * Time.deltaTime);
            }
            if (currentAction.enableTargetMatching)
            {
                // if (player.animator.isMatchingTarget)
                // {
                //     return;
                // }
                MatchTarget(currentAction);
            }
        }
    }

    public void HandleActions()
    {
        HandleObstacleAction();
        HandleLedgeJump();
    }

    private void HandleObstacleAction()
    {
        if (hitData.forwardHitFound)
        {
            if (!player.isPerformingAction)
            {
                foreach (var action in parkourActions)
                {
                    if (action.CheckIfPossible(hitData, player.transform))
                    {
                        currentAction = action;
                        GameObject sphere = Instantiate(debugSphere, action.MatchPos, quaternion.identity);
                        Destroy(sphere, 2f);

                        // StartCoroutine(PerformParkourAction(action));
                        player.characterController.enabled = false;
                        player.playerAnimatorManager.PlayTargetParkourAnimation(action.animation, true);
                    }
                }
            }
        }
        else
        {
            currentAction = null;
        }
    }

    private void HandleLedgeJump()
    {
        if (player.isOnLedge && !player.isPerformingAction && !hitData.forwardHitFound)
        {
            if (player.ledgeData.angle <= 50)
            {
                player.characterController.enabled = false;
                player.playerAnimatorManager.PlayTargetAnimation(JumpDownAction.animation, true);
            }
        }
    }

    private IEnumerator PerformParkourAction(ParkourAction action)
    {
        // player.characterController.excludeLayers = obstacleLayer;
        player.characterController.enabled = false;
        player.playerAnimatorManager.PlayTargetParkourAnimation(action.animation, true);
        yield return null;

        var animationState = player.animator.GetNextAnimatorStateInfo(0);

        float timer = 0f;
        while (timer <= animationState.length)
        {
            timer += Time.deltaTime;
            if (action.rotateToObstacle)
            {
                player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, action.targetRotation, action.rotationSpeed * Time.deltaTime);
            }
            if (action.enableTargetMatching)
            {
                MatchTarget(action);
            }
            yield return null;
        }

        yield return new WaitForSeconds(action.postDelay);
        player.characterController.enabled = true;
    }

    void MatchTarget(ParkourAction action)
    {
        player.animator.MatchTarget(action.MatchPos, player.transform.rotation, action.matchBodyPart,
        new MatchTargetWeightMask(action.matchPosWeight, 0), action.matchStartTime, action.matchTargetTime);
    }
}