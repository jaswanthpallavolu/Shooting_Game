using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/Parkour Action")]
public class ParkourAction : ScriptableObject
{
    public string obstacleTag;
    public string animation;
    [SerializeField] float minHeight = 0.3f;
    [SerializeField] float maxHeight = 1f;

    [Header("Rotate")]
    public bool rotateToObstacle = false;
    public float rotationSpeed = 2f;
    public Quaternion targetRotation;

    [Header("Target Matching")]
    public bool enableTargetMatching = true;
    public AvatarTarget matchBodyPart;
    public float matchStartTime;
    public float matchTargetTime;
    public Vector3 matchPosWeight;
    public float postDelay;

    // public Vector3 MatchPos { get; set; }
    public Vector3 MatchPos;


    public bool CheckIfPossible(ObstacleHitData hitData, Transform player)
    {
        if (!string.IsNullOrEmpty(obstacleTag) && hitData.forwardHit.transform.tag != obstacleTag)
        {
            return false;
        }

        float height = hitData.heightHit.point.y - player.position.y;
        if (height < minHeight || height > maxHeight) return false;

        if (rotateToObstacle)
        {
            targetRotation = Quaternion.LookRotation(-hitData.forwardHit.normal);
        }
        if (enableTargetMatching)
        {
            MatchPos = hitData.heightHit.point;
        }

        return true;
    }

}