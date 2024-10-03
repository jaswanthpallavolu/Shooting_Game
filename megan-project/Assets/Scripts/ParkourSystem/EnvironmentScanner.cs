using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    PlayerManager player;
    [SerializeField] Vector3 forwardRayOffset = new Vector3(0, 0.25f, 0);
    [SerializeField] float forwardRayLength = .8f;
    [SerializeField] float heightRayLength = 5f;
    [SerializeField] float ledgeRayLength = 10f;
    [SerializeField] float surfaceRayLength = 4f;
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

    public bool LedgeCheck(Vector3 moveDir, out LedgeData ledgeData)
    {
        ledgeData = new LedgeData();
        if (moveDir.magnitude == 0)
        {
            moveDir = player.transform.forward;
            return false;
        }
        float originOffset = 0.5f;
        Vector3 origin = player.transform.position + Vector3.up + moveDir * originOffset;
        bool hitFound = PhysicsUtil.ThreeRayCast(origin, moveDir, .25f, out List<RaycastHit> hits, obstacleLayer, ledgeRayLength, player.transform, true);
        if (hitFound)
        {
            var validHits = hits.Where(h => player.transform.position.y - h.point.y > ledgeHeightThreshold);

            for (int i = 0; i < validHits.Count(); i++)
            {
                Vector3 surfaceOrigin = validHits.ElementAt(i).point;
                surfaceOrigin.y = player.transform.position.y - 0.2f;

                bool surfaceHitFound = Physics.Raycast(surfaceOrigin, player.transform.position - surfaceOrigin, out RaycastHit surfaceHit, surfaceRayLength, obstacleLayer);
                Debug.DrawRay(surfaceOrigin, player.transform.position - surfaceOrigin * surfaceRayLength, Color.green);
                if (surfaceHitFound)
                {
                    float height = player.transform.position.y - validHits.ElementAt(i).point.y;
                    ledgeData.angle = Vector3.Angle(player.transform.forward, surfaceHit.normal);
                    ledgeData.height = height;
                    ledgeData.surfaceHit = surfaceHit;
                    return true;
                }
            }


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

public struct LedgeData
{
    public float height;
    public float angle;
    public RaycastHit surfaceHit;
}