using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    EnvironmentScanner environmentScanner;
    PlayerManager player;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] List<ParkourAction> parkourActions;
    public ParkourAction currentAction;
    [SerializeField] int parkourAnimationLayer = 2;
    [SerializeField] GameObject debugSphere;

    void Awake()
    {
        environmentScanner = GetComponent<EnvironmentScanner>();
        player = GetComponent<PlayerManager>();
    }

    void Update()
    {

        var hitData = environmentScanner.ObstacleCheck();
        if (hitData.forwardHitFound)
        {
            // Debug.Log("Obstacle found: " + hitData.forwardHit.transform.name);
            if (Input.GetKeyDown(KeyCode.Space) && !player.isPerformingAction)
            {
                foreach (var action in parkourActions)
                {
                    if (action.CheckIfPossible(hitData, player.transform))
                    {


                        if (action.rotateToObstacle)
                        {
                            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, action.targetRotation, action.rotationSpeed * Time.deltaTime);
                        }
                        currentAction = action;
                        GameObject sphere = Instantiate(debugSphere, action.MatchPos + Vector3.forward * .5f, quaternion.identity);
                        Destroy(sphere, 5f);
                        StartCoroutine(PerformParkourAction(action));
                        // player.playerAnimatorManager.PlayTargetParkourAnimation(action.animation, true);
                        // player.characterController.enabled = false;



                    }
                }
            }
        }

        // if (currentAction != null && player.animator.GetCurrentAnimatorStateInfo(parkourAnimationLayer).IsName(currentAction.animation))
        // {
        //     if (currentAction.enableTargetMatching)
        //     {
        //         // if (player.animator.isMatchingTarget)
        //         // {
        //         //     return;
        //         // }

        //         MatchTarget(currentAction);
        //     }
        // }
    }

    private IEnumerator PerformParkourAction(ParkourAction action)
    {
        // player.characterController.excludeLayers = obstacleLayer;
        player.playerAnimatorManager.PlayTargetParkourAnimation(action.animation, true);
        player.characterController.enabled = false;
        yield return null;

        var animationState = player.animator.GetNextAnimatorStateInfo(0);

        float timer = 0f;
        while (timer <= animationState.length)
        {
            timer += Time.deltaTime;

            if (action.enableTargetMatching)
            {
                MatchTarget(action);
            }
            yield return null;
        }

        // player.characterController.enabled = true;
    }

    void MatchTarget(ParkourAction action)
    {
        player.animator.MatchTarget(action.MatchPos + Vector3.forward * .5f, player.transform.rotation, action.matchBodyPart,
        new MatchTargetWeightMask(action.matchPosWeight, 0), action.matchStartTime, action.matchTargetTime);
        Debug.Log("matching..");
    }
}