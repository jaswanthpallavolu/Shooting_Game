using System.Collections;
using System.Collections.Generic;
using Ballistics;
using Ballistics1;
using UnityEngine;

public class WeaponModelInstantiationSlot : MonoBehaviour
{
    public WeaponModelSlot weaponSlot;
    public GameObject currentWeaponModel;

    public void UnLoadWeapon()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }

    public void OnLoadWeapon(GameObject weaponModel)
    {
        currentWeaponModel = weaponModel;
        weaponModel.transform.parent = transform;

        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
        weaponModel.transform.localScale = Vector3.one;
    }

    public GameObject OnLoadWeapon1(GameObject weaponModel)
    {
        weaponModel = Instantiate(weaponModel, transform, false);
        currentWeaponModel = weaponModel;
        WeaponManager weaponManager = weaponModel.GetComponent<WeaponManager>();
        // weaponManager.We.BulletSpawnPoint = Camera.main.transform;
        weaponManager.Initiliaze();
        // Weapon weapon = weaponModel.GetComponent<Weapon>();
        // if (weapon)
        // {
        //     weapon.BulletSpawnPoint = Camera.main.transform;
        // }
        return weaponModel;
    }
}