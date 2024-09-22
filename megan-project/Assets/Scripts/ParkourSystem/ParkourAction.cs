using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/Parkour Action")]
public class ParkourAction : ScriptableObject
{
    public string animation;
    public float minHeight = 0.3f;
    public float maxHeight = 1f;

    public bool CheckIfPossible(ObstacleHitData hitData, Transform player)
    {
        Debug.Log(hitData.heightHit.point);
        float height = hitData.heightHit.point.y - player.position.y;
        if (height < minHeight || height > maxHeight) return false;

        player.transform.LookAt(-hitData.forwardHit.transform.forward);

        return true;
    }

}