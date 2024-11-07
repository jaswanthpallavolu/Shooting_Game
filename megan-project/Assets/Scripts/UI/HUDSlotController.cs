using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HUDSlotController : MonoBehaviour
{
    public HUDWeaponManager hudWeaponManager;
    public SlotSection slotSection;
    public HUDWeaponSlot[] hudWeaponSlots;
    [SerializeField] Sprite[] weaponItems;
    List<WeaponItemData> itemsData;

    [Header("FireArm Slots")]
    public int[] equippedSlotItemIndexes;
    public int[] unequippedSlotItemIndexes;

    void Awake()
    {
        hudWeaponSlots = GetComponentsInChildren<HUDWeaponSlot>();
        hudWeaponManager = GetComponentInParent<HUDWeaponManager>();
        itemsData = new List<WeaponItemData>();
        for (int i = 0; i < weaponItems.Length; i++)
        {
            itemsData.Add(new WeaponItemData { ID = slotSection.ToString() + i, Icon = weaponItems[i], Name = slotSection.ToString() + i });
        }
        equippedSlotItemIndexes = new int[] { 0, 1 };
        LoadWeaponSlots();
    }

    // Start is called before the first frame update
    void Start()
    {

        // FilterUnequippedItemIndexes();

        // Assign Button navigation to all weapon slots.
        // AssignSlotButtonNavigation();


    }

    // Update is called once per frame
    void Update()
    {
    }

    void LoadWeaponSlots()
    {
        if (slotSection == SlotSection.UP || slotSection == SlotSection.DOWN)
        {
            for (int i = 0; i < hudWeaponSlots.Count(); i++)
            {
                hudWeaponSlots[i].slotIndex = i;
                if (itemsData.Count() > i)
                {
                    hudWeaponSlots[i].gameObject.SetActive(true);
                    hudWeaponSlots[i].LoadProjectileSlot(itemsData[i], hudWeaponManager.HUD_Weapon_Item);
                }
                else
                {
                    hudWeaponSlots[i].gameObject.SetActive(false);
                }
            }
        }
        else if (slotSection == SlotSection.LEFT || slotSection == SlotSection.RIGHT)
        {
            for (int i = 0; i < hudWeaponSlots.Count(); i++)
            {
                hudWeaponSlots[i].slotIndex = i;
                if (itemsData.Count() > i)
                {
                    hudWeaponSlots[i].gameObject.SetActive(true);
                    hudWeaponSlots[i].LoadFireArmSlot(itemsData, hudWeaponManager.HUD_Weapon_Item, equippedSlotItemIndexes[i]);
                }
                else
                {
                    hudWeaponSlots[i].gameObject.SetActive(false);
                }
            }
        }
    }

    void FilterUnequippedItemIndexes()
    {
        unequippedSlotItemIndexes = new int[weaponItems.Length];
        int[] temp = new int[weaponItems.Length];
        for (int i = 0; i < weaponItems.Length; i++)
        {
            temp[i] = i;
        }

        HashSet<int> a, b;
        a = equippedSlotItemIndexes.ToHashSet();
        b = temp.ToHashSet();

        foreach (var added in a.Except(b).ToArray())
        {
            Debug.Log("added " + added);
        }
        foreach (var removed in b.Except(a).ToArray())
        {
            Debug.Log("removed " + removed);
        }

    }

    public void HandleSlotItemIndexes(int slotIndex, int itemIndex)
    {
        var oppositeSlot = hudWeaponSlots[0].slotIndex == slotIndex ? hudWeaponSlots[1] : hudWeaponSlots[0];
        int oppositeSlotCurrIndex = oppositeSlot.scrollView.currIndex;

        if (oppositeSlotCurrIndex == itemIndex)
        {
            if (oppositeSlotCurrIndex == 0) oppositeSlotCurrIndex += 1;
            else oppositeSlotCurrIndex -= 1;
            // oppositeSlot.scrollView.SetCurrentItemIndex(oppositeSlotCurrIndex);
            oppositeSlot.scrollView.currIndex = oppositeSlotCurrIndex;
        }
    }

    void AssignSlotButtonNavigation()
    {
        for (int i = 0; i < hudWeaponSlots.Length; i++)
        {
            var navigation = hudWeaponSlots[i].GetComponent<Button>().navigation;
            foreach (var navigateWeaponSlot in hudWeaponManager.navigateWeaponSlots)
            {
                if (navigateWeaponSlot.SlotSection == SlotSection.UP)
                {
                    if (slotSection == SlotSection.UP && i + 1 < hudWeaponSlots.Length)
                    {
                        navigation.selectOnUp = hudWeaponSlots[i + 1].GetComponent<Button>();
                    }
                    else
                    {
                        navigation.selectOnUp = navigateWeaponSlot.FirstHUDWeaponSlot.GetComponent<Button>();
                    }
                }
                else if (navigateWeaponSlot.SlotSection == SlotSection.DOWN)
                {
                    if (slotSection == SlotSection.DOWN && i + 1 < hudWeaponSlots.Length)
                    {
                        navigation.selectOnDown = hudWeaponSlots[i + 1].GetComponent<Button>();
                    }
                    else
                    {
                        navigation.selectOnDown = navigateWeaponSlot.FirstHUDWeaponSlot.GetComponent<Button>();
                    }
                }
                else if (navigateWeaponSlot.SlotSection == SlotSection.LEFT)
                {
                    if (slotSection == SlotSection.LEFT && i + 1 < hudWeaponSlots.Length)
                    {
                        navigation.selectOnLeft = hudWeaponSlots[i + 1].GetComponent<Button>();
                    }
                    else
                    {
                        navigation.selectOnLeft = navigateWeaponSlot.FirstHUDWeaponSlot.GetComponent<Button>();
                    }
                }
                else if (navigateWeaponSlot.SlotSection == SlotSection.RIGHT)
                {
                    if (slotSection == SlotSection.RIGHT && i + 1 < hudWeaponSlots.Length)
                    {
                        navigation.selectOnRight = hudWeaponSlots[i + 1].GetComponent<Button>();
                    }
                    else
                    {
                        navigation.selectOnRight = navigateWeaponSlot.FirstHUDWeaponSlot.GetComponent<Button>();
                    }
                }
            }
            hudWeaponSlots[i].GetComponent<Button>().navigation = navigation;
        }
    }

    // NAVIGATION
    public void Navigate()
    {
        int totalSlots = hudWeaponSlots.Length;
        if (totalSlots == 0) return;
        int nextSlotIndex = getNextSlotIndex(hudWeaponManager.selectedSlotIndex, totalSlots, slotSection);
        hudWeaponSlots[nextSlotIndex].SelectWeaponSlot();
    }

    int getNextSlotIndex(int slotIndex, int totalSlots, SlotSection slotSection)
    {
        if (hudWeaponManager.selectedSlotSection == slotSection) slotIndex++;
        else slotIndex = 0;
        if (slotIndex == totalSlots) slotIndex = 0;
        return slotIndex;
    }
}

public struct WeaponItemData
{
    public string ID { get; set; }
    public Sprite Icon { get; set; }
    public string Name { get; set; }
}
