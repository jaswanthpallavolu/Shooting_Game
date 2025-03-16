using System.Collections;
using System.Collections.Generic;
using Ballistics;
using SG_Project;
using UnityEngine;

public class WeaponSpread : MonoBehaviour
{
    [Header("Spread")]
    public Vector3 baseSpread = new Vector3(0.1f, 0.1f, 0.1f);
    public Vector3 direction;
    public float maxSpreaad = 2f;
    public float spread = 0f;
    public float correctionTime = 0.25f;
    public float currSpread = 0f;

    void Update()
    {
        if (spread > 0)
        {
            spread = spread - (Time.deltaTime / correctionTime);
        }
        else
        {
            spread = 0;
        }
    }

    public void BulletFired()
    {
        // timer = RecoveryTime;
        // currentShot++;
        ResetSpread();
    }

    public Vector3 CalculateShootDirection(Weapon weapon)
    {
        currSpread = spread;
        direction = new Vector3(Random.Range(-baseSpread.x, baseSpread.x), Random.Range(-baseSpread.y, baseSpread.y), Random.Range(-baseSpread.z, baseSpread.z));
        return weapon.BulletSpawnPoint.forward + direction * currSpread;
    }

    public void ResetSpread()
    {
        spread = maxSpreaad;
    }
}