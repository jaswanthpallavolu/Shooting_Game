using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldItemDatabase : MonoBehaviour
{
    public static WorldItemDatabase instance;
    [SerializeField] MeleeWeaponItem[] meleeWeapons;
    [SerializeField] FirearmWeaponItem[] firearmWeapons;
    public MeleeWeaponItem unarmedWeapon;
    private List<WeaponItem> weaponItems = new List<WeaponItem>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // foreach (var weapons in meleeWeapons.Union(meleeWeapons).ToArray())
        // {
        //     foreach (var weapon in weapons)
        //     {
        //         weaponItems.Add(weapon);
        //     }
        // }
        foreach (var weapon in meleeWeapons)
        {
            weaponItems.Add(weapon);
        }
        foreach (var weapon in firearmWeapons)
        {
            weaponItems.Add(weapon);
        }

        for (int i = 0; i < weaponItems.Count; i++)
        {
            weaponItems[i].itemId = i;
        }
    }

    public WeaponItem GetWeaponById(float id)
    {
        return weaponItems.FirstOrDefault(weapon => weapon.itemId == id);
    }

}
