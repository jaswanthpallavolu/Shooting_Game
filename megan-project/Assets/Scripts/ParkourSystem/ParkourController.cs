using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    EnvironmentScanner environmentScanner;
    PlayerManager player;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] List<ParkourAction> parkourActions;

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
            Debug.Log("Obstacle found: " + hitData.forwardHit.transform.name);
            if (Input.GetKeyDown(KeyCode.Space) && !player.isPerformingAction)
            {
                foreach (var action in parkourActions)
                {
                    if (action.CheckIfPossible(hitData, player.transform))
                    {
                        player.characterController.excludeLayers = obstacleLayer;
                        player.playerAnimatorManager.PlayTargetParkourAnimation("Obstacle_Vault", true);
                    }
                }
            }
        }
    }
}