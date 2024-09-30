using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    PlayerManager player;
    [SerializeField] Vector3 forwardRayOffset = new Vector3(0, 0.25f, 0);
    [SerializeField] float forwardRayLength = .8f;
    [SerializeField] float heightRayLength = 5f;
    [SerializeField] float ledgeRayLength = 10f;
    [SerializeField] float ledgeHeightThreshold = .75f;
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

    public bool LedgeCheck(Vector3 moveDir)
    {
        if (moveDir == Vector3.zero) return false;
        float originOffset = 0.5f;
        Vector3 origin = player.transform.position + Vector3.up + moveDir * originOffset;
        bool hitFound = Physics.Raycast(origin, Vector3.down, out RaycastHit hit, ledgeRayLength, obstacleLayer);
        Debug.DrawRay(origin, Vector3.down * ledgeRayLength, hitFound ? Color.green : Color.red);
        if (hitFound)
        {

            float height = player.transform.position.y - hit.transform.position.y;
            if (height > ledgeHeightThreshold)
            {
                return true;
            }
            return false;
        }
        return false;
    }
}

public struct ObstacleHitData
{
    public bool forwardHitFound;
    public bool heightHitFound;
    public RaycastHit forwardHit;
    public RaycastHit heightHit;
}