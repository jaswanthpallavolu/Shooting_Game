using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsUtil
{
    public static bool ThreeRayCast(Vector3 origin, Vector3 dir, float spacing,
    out List<RaycastHit> hits, LayerMask layerMask, float distance, Transform tranfrom, bool debug = false)
    {
        bool centerHitFound = Physics.Raycast(origin, Vector3.down, out RaycastHit centerHit, distance, layerMask);
        bool leftHitFound = Physics.Raycast(origin - tranfrom.right * spacing, Vector3.down, out RaycastHit leftHit, distance, layerMask);
        bool rightHitFound = Physics.Raycast(origin + tranfrom.right * spacing, Vector3.down, out RaycastHit rightHit, distance, layerMask);

        bool hitFound = centerHitFound || leftHitFound || rightHitFound;
        hits = new List<RaycastHit>() { centerHit, leftHit, rightHit };
        if (hitFound && debug)
        {
            Debug.DrawLine(origin, centerHit.point, Color.red);
            Debug.DrawLine(origin - tranfrom.right * spacing, leftHit.point, Color.red);
            Debug.DrawLine(origin + tranfrom.right * spacing, rightHit.point, Color.red);
        }

        return hitFound;
    }
}
