using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUDWeaponManager : MonoBehaviour
{
    [Header("Selected Weapon")]
    public HUDWeaponSlot selectedSlot;
    public WeaponItemData selectedWeaponItem;
    public string selectedWeaponItemID;

    [Header("Prefab")]
    public GameObject HUD_Weapon_Item;

    [SerializeField] HUDSlotController[] hudSlotControllers;
    public NavigateWeaponSlot[] navigateWeaponSlots = new NavigateWeaponSlot[4];

    void Awake()
    {
        hudSlotControllers = GetComponentsInChildren<HUDSlotController>();

        int slotIndex = 0;
        foreach (var hudSlotController in hudSlotControllers)
        {
            navigateWeaponSlots[slotIndex] = new NavigateWeaponSlot
            {
                WeaponSlot = hudSlotController.weaponSlot,
                FirstHUDWeaponSlot = hudSlotController.GetComponentInChildren<HUDWeaponSlot>()
            };
            slotIndex++;
        }
    }

    void Start()
    {
        // PreselectWeaponItem();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCurrentWeaponItem(HUDWeaponSlot slot)
    {
        if (slot != null)
        {
            selectedSlot = slot;
            selectedWeaponItem = slot.currentSlotItem.weaponItemData;
            selectedWeaponItemID = slot.currentSlotItem.weaponItemData.ID;
        }
        // // prevent outside mouse clicks
        // else if (selectedSlot != null)
        // {
        //     EventSystem.current.SetSelectedGameObject(selectedSlot.gameObject);
        // }
    }

    // Avoid outside mouse clicks 
    public void PreselectWeaponItem()
    {
        return;
        if (selectedSlot != null)
        {
            if (EventSystem.current.currentSelectedGameObject == selectedSlot.gameObject) return;
            EventSystem.current.SetSelectedGameObject(selectedSlot.gameObject);
        }
    }
}

public enum WeaponSlot
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

// public enum WeaponSlotType
// {
//     PROJECTILE, LONGARM, SIDEARM
// }

public struct NavigateWeaponSlot
{
    public WeaponSlot WeaponSlot { set; get; }
    public HUDWeaponSlot FirstHUDWeaponSlot { set; get; }
}
