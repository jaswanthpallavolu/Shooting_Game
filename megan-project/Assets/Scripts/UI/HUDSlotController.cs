using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HUDSlotController : MonoBehaviour
{
    public HUDWeaponManager hudWeaponManager;
    public WeaponSlot weaponSlot;
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
            itemsData.Add(new WeaponItemData { ID = weaponSlot.ToString() + i, Icon = weaponItems[i], Name = weaponSlot.ToString() + i });
        }
        equippedSlotItemIndexes = new int[] { 0, 1 };
        LoadWeaponSlots();
    }

    // Start is called before the first frame update
    void Start()
    {

        // FilterUnequippedItemIndexes();

        // Assign Button navigation to all weapon slots.
        AssignSlotButtonNavigation();


    }

    // Update is called once per frame
    void Update()
    {
    }

    void LoadWeaponSlots()
    {
        if (weaponSlot == WeaponSlot.UP || weaponSlot == WeaponSlot.DOWN)
        {
            for (int i = 0; i < hudWeaponSlots.Count(); i++)
            {
                hudWeaponSlots[i].slotID = i;
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
        else if (weaponSlot == WeaponSlot.LEFT || weaponSlot == WeaponSlot.RIGHT)
        {
            for (int i = 0; i < hudWeaponSlots.Count(); i++)
            {
                hudWeaponSlots[i].slotID = i;
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

    public void HandleSlotItemIndexes(int slotID, int itemIndex)
    {
        var oppositeSlot = hudWeaponSlots[0].slotID == slotID ? hudWeaponSlots[1] : hudWeaponSlots[0];
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
                if (navigateWeaponSlot.WeaponSlot == WeaponSlot.UP)
                {
                    if (weaponSlot == WeaponSlot.UP && i + 1 < hudWeaponSlots.Length)
                    {
                        navigation.selectOnUp = hudWeaponSlots[i + 1].GetComponent<Button>();
                    }
                    else
                    {
                        navigation.selectOnUp = navigateWeaponSlot.FirstHUDWeaponSlot.GetComponent<Button>();
                    }
                }
                else if (navigateWeaponSlot.WeaponSlot == WeaponSlot.DOWN)
                {
                    if (weaponSlot == WeaponSlot.DOWN && i + 1 < hudWeaponSlots.Length)
                    {
                        navigation.selectOnDown = hudWeaponSlots[i + 1].GetComponent<Button>();
                    }
                    else
                    {
                        navigation.selectOnDown = navigateWeaponSlot.FirstHUDWeaponSlot.GetComponent<Button>();
                    }
                }
                else if (navigateWeaponSlot.WeaponSlot == WeaponSlot.LEFT)
                {
                    if (weaponSlot == WeaponSlot.LEFT && i + 1 < hudWeaponSlots.Length)
                    {
                        navigation.selectOnLeft = hudWeaponSlots[i + 1].GetComponent<Button>();
                    }
                    else
                    {
                        navigation.selectOnLeft = navigateWeaponSlot.FirstHUDWeaponSlot.GetComponent<Button>();
                    }
                }
                else if (navigateWeaponSlot.WeaponSlot == WeaponSlot.RIGHT)
                {
                    if (weaponSlot == WeaponSlot.RIGHT && i + 1 < hudWeaponSlots.Length)
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
}

public struct WeaponItemData
{
    public string ID { get; set; }
    public Sprite Icon { get; set; }
    public string Name { get; set; }
}
