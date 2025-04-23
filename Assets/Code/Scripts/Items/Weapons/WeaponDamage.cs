using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class WeaponDamage : MonoBehaviour
{
    public CharacterManager CharacterCausingDamage { set; get; }
    public float weaponRange = 300f;
    public Transform rigAimTarget;

    protected virtual void Awake()
    {
    }

    protected virtual void Update()
    {
        rigAimTarget = CharacterCausingDamage.characterAnimatorManager.currentRigAimTarget;
        RaycastWeapon();
    }

    protected virtual void RaycastWeapon()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Vector3 aimPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, CharacterCausingDamage.aimOffsetRange));
        // rigAimTarget.localPosition = new Vector3(rigAimTarget.localPosition.x, aimPosition.y, rigAimTarget.localPosition.z);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Vector3 origin = new Vector3(screenCenterPoint.x, screenCenterPoint.y, 0);
        Debug.Log(ray.origin);
        Debug.DrawLine(ray.origin, origin + new Vector3(0, 0, CharacterCausingDamage.aimOffsetRange), Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, CharacterCausingDamage.aimOffsetRange))
        {
            Debug.Log(hit.point);
            aimPosition = hit.point;
        }
        rigAimTarget.position = aimPosition;
    }

    void FireWeapon()
    {
        // hitInfo
    }


}
