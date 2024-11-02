using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDWeaponSlot : MonoBehaviour, IMoveHandler, ISelectHandler, IDeselectHandler
{
    public int slotID;
    public HUDWeaponSlot hudWeaponSlot;
    public HUDSlotController slotController;
    public Hud_Weapon_Item currentSlotItem;
    [SerializeField] private GameObject selectedRect;

    [Header("Custom Scroll")]
    public HUDWeaponScrollView scrollView;
    public bool enableWeaponSwap = false;
    public bool slotSelected = false;
    public bool isSlotSelected = false;




    // Start is called before the first frame update
    void Awake()
    {
        slotController = GetComponentInParent<HUDSlotController>();
        hudWeaponSlot = GetComponent<HUDWeaponSlot>();
    }

    // Update is called once per frame
    void Update()
    {
        isSlotSelected = hudWeaponSlot == slotController.hudWeaponManager.selectedSlot;
        SetItemSelected(isSlotSelected);

        if (slotController.weaponSlot == WeaponSlot.LEFT || slotController.weaponSlot == WeaponSlot.RIGHT)
        {
            if (Input.GetKeyDown(KeyCode.V) && isSlotSelected)
            {
                enableWeaponSwap = true;
                GetComponent<Button>().enabled = false;
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                enableWeaponSwap = false;
                GetComponent<Button>().enabled = true;
            }
        }
    }

    public void LoadProjectileSlot(WeaponItemData weaponItem, GameObject HUD_Weapon_Item)
    {
        var spawnedItem = Instantiate(HUD_Weapon_Item, transform);
        spawnedItem.GetComponent<Hud_Weapon_Item>().SetItemImage(weaponItem.Icon);
        spawnedItem.GetComponent<Hud_Weapon_Item>().weaponItemData = weaponItem;
        currentSlotItem = spawnedItem.GetComponent<Hud_Weapon_Item>();
    }

    public void LoadFireArmSlot(List<WeaponItemData> weaponItems, GameObject HUD_Weapon_Item, int weaponIndex)
    {
        scrollView.items = weaponItems;
        scrollView.height = 100f;
        scrollView.currIndex = weaponIndex;
        // scrollView.SetCurrentItemIndex(weaponIndex);
        foreach (var weaponItem in weaponItems)
        {
            var spawnedItem = Instantiate(HUD_Weapon_Item, scrollView.content.transform);
            spawnedItem.GetComponent<RectTransform>().sizeDelta = new Vector2(175f, 100f);
            spawnedItem.GetComponent<Hud_Weapon_Item>().SetItemImage(weaponItem.Icon);
            spawnedItem.GetComponent<Hud_Weapon_Item>().weaponItemData = weaponItem;
        }

    }

    public void SetCurrentSlotItem(Hud_Weapon_Item slotItem, int slotItemIndex = -1)
    {
        currentSlotItem = slotItem;
        // if (slotItemIndex != -1) slotController.HandleSlotItemIndexes(slotID, slotItemIndex);
    }
    public void SetItemSelected(bool isSelected)
    {
        if (isSelected)
        {
            selectedRect.SetActive(true);
        }
        else
        {
            selectedRect.SetActive(false);
        }
    }


    // EVENTS
    public void OnMove(AxisEventData eventData)
    {

        // throw new System.NotImplementedException();
    }

    public void OnSelect(BaseEventData eventData)
    {
        slotController.hudWeaponManager.SetCurrentWeaponItem(hudWeaponSlot);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // Debug.Log("Deselected " + currentSlotItem.weaponItemData.Name);
        slotController.hudWeaponManager.PreselectWeaponItem();
    }
}
