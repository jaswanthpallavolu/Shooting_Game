using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    PlayerManager player;
    [SerializeField] Vector3 forwardRayOffset = new Vector3(0, 0.25f, 0);
    [SerializeField] float forwardRayLength = .8f;
    [SerializeField] float heightRayLength = 5f;
    [SerializeField] LayerMask obstacleLayer;

    void Awake()
    {
        player = GetComponent<PlayerManager>();
    }
    public ObstacleHitData ObstacleCheck()
    {
        ObstacleHitData hitData = new ObstacleHitData();
        Vector3 forwardOrigin = player.transform.position + forwardRayOffset;
        hitData.forwardHitFound = Physics.Raycast(forwardOrigin, player.transform.forward, out hitData.forwardHit, forwardRayLength, obstacleLayer);

        Debug.DrawRay(forwardOrigin, player.transform.forward * forwardRayLength, hitData.forwardHitFound ? Color.red : Color.white);
        if (hitData.forwardHitFound)
        {
            Vector3 heightOrigin = hitData.forwardHit.point + Vector3.up * heightRayLength;
            hitData.heightHitFound = Physics.Raycast(heightOrigin, Vector3.down, out hitData.heightHit, heightRayLength, obstacleLayer);
            // Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength, hitData.heightHitFound ? Color.red : Color.white);
            Debug.DrawLine(heightOrigin, hitData.heightHit.point);
        }
        return hitData;
    }
}

public struct ObstacleHitData
{
    public bool forwardHitFound;
    public bool heightHitFound;
    public RaycastHit forwardHit;
    public RaycastHit heightHit;
}