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
        rigAimTarget.position = aimPosition;

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        // if(Physics.Raycast(ray, out RaycastHit hit, weaponRange)){

        // }
    }

    void FireWeapon()
    {
        // hitInfo
    }


}
