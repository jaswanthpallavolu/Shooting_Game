using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUDWeaponManager : MonoBehaviour
{
    [Header("Selected Weapon")]
    public int selectedSlotIndex;
    public SlotSection selectedSlotSection;
    public HUDWeaponSlot selectedSlot;
    public WeaponItemData selectedWeaponItem;
    public string selectedWeaponItemID;

    [Header("Prefab")]
    public GameObject HUD_Weapon_Item;

    [Header("Slot Controllers")]
    [SerializeField] HUDSlotController upSlotController;
    [SerializeField] HUDSlotController downSlotController;
    [SerializeField] HUDSlotController leftSlotController;
    [SerializeField] HUDSlotController rightSlotController;

    [Header("Quick Swap")]
    [SerializeField] List<HUDWeaponSlot> quickSwapList;
    [SerializeField] int quickSwapIndex = -1;
    int leftSlotsTotal;
    int rightSlotsTotal;

    [Header("skip")]
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
                SlotSection = hudSlotController.slotSection,
                FirstHUDWeaponSlot = hudSlotController.GetComponentInChildren<HUDWeaponSlot>()
            };
            slotIndex++;
        }
    }

    void Start()
    {
        // PreselectWeaponItem();
        SetQuickSwapList();

        // initialize selection
        rightSlotController.GetComponentInChildren<HUDWeaponSlot>().SelectWeaponSlot();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateQuickSwapIndex();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PerformQuickSwap();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            upSlotController.Navigate();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            downSlotController.Navigate();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            leftSlotController.Navigate();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rightSlotController.Navigate();
        }


    }

    void UpdateQuickSwapIndex()
    {
        if (selectedSlotSection == SlotSection.RIGHT) quickSwapIndex = leftSlotsTotal + selectedSlotIndex;
        if (selectedSlotSection == SlotSection.LEFT) quickSwapIndex = leftSlotsTotal - 1 - selectedSlotIndex;
    }

    void PerformQuickSwap()
    {
        int totalSlots = quickSwapList.Count;
        if (totalSlots == 0) return;

        if (selectedSlotSection == SlotSection.LEFT || selectedSlotSection == SlotSection.RIGHT) quickSwapIndex++;
        else quickSwapIndex = 0;
        if (quickSwapIndex == quickSwapList.Count) quickSwapIndex = 0;

        quickSwapList[quickSwapIndex].SelectWeaponSlot();
    }

    void SetQuickSwapList()
    {
        HUDWeaponSlot[] leftWeaponSlots = leftSlotController.GetComponentsInChildren<HUDWeaponSlot>();
        HUDWeaponSlot[] rightWeaponSlots = rightSlotController.GetComponentsInChildren<HUDWeaponSlot>();
        quickSwapList = new List<HUDWeaponSlot>();
        leftSlotsTotal = leftWeaponSlots.Length;
        rightSlotsTotal = rightWeaponSlots.Length;

        for (int i = leftSlotsTotal - 1; i >= 0; i--)
        {
            quickSwapList.Add(leftWeaponSlots[i]);
        }
        for (int i = 0; i < rightSlotsTotal; i++)
        {
            quickSwapList.Add(rightWeaponSlots[i]);
        }
    }

    public void SetCurrentWeaponItem(HUDWeaponSlot slot)
    {
        if (slot != null)
        {
            selectedSlot = slot;
            selectedSlotIndex = slot.slotIndex;
            selectedSlotSection = slot.slotController.slotSection;
            if (slot.currentSlotItem != null)
            {
                selectedWeaponItem = slot.currentSlotItem.weaponItemData;
                selectedWeaponItemID = slot.currentSlotItem.weaponItemData.ID;
            }

        }
        // // prevent outside mouse clicks
        // else if (selectedSlot != null)
        // {
        //     EventSystem.current.SetSelectedGameObject(selectedSlot.gameObject);
        // }
    }
}

public enum SlotSection
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

// public enum SlotSection
// {
//     PROJECTILE, LONGARM, SIDEARM
// }

public struct NavigateWeaponSlot
{
    public SlotSection SlotSection { set; get; }
    public HUDWeaponSlot FirstHUDWeaponSlot { set; get; }
}
