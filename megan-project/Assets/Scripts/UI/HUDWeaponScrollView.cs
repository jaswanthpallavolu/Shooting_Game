using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDWeaponScrollView : MonoBehaviour
{
    [SerializeField] HUDWeaponSlot hudWeaponSlot;
    [SerializeField] ScrollViewNavigation scrollViewNavigation;
    [SerializeField] RectTransform viewport;
    [SerializeField] public RectTransform content;

    [SerializeField] public List<WeaponItemData> items;
    public Hud_Weapon_Item[] hud_Weapon_Items;
    public int currIndex = 0;

    [SerializeField] public float height;
    [SerializeField] float scrollSpeed = 4f;

    void Awake()
    {
        hudWeaponSlot = GetComponentInParent<HUDWeaponSlot>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // height = items[0]   
        // rectTransform.rect.height;

        // rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottom);
        // rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, top);
        // hud_Weapon_Items = content.GetComponentsInChildren<Hud_Weapon_Item>();
    }


    // Update is called once per frame
    void Update()
    {
        if (hud_Weapon_Items.Length == 0)
        {
            hud_Weapon_Items = content.GetComponentsInChildren<Hud_Weapon_Item>();
        }

        scrollViewNavigation.CurrIndex = currIndex;
        scrollViewNavigation.TotalCount = hud_Weapon_Items.Length;

        if (hudWeaponSlot.enableWeaponSwap)
        {
            viewport.GetComponent<RectMask2D>().enabled = false;
            Up();
            Down();
        }
        else
        {
            viewport.GetComponent<RectMask2D>().enabled = true;
            ScrollToWeapon(currIndex, false);

        }
    }

    void Up()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (currIndex == 0) return;
            currIndex--;
            // StartCoroutine(MoveFromTo(content.offsetMax, new Vector2(content.anchoredPosition.x, -height * currIndex), scrollSpeed, content));
            ScrollToWeapon(currIndex);
            hudWeaponSlot.HandleWeaponSlotItem(currIndex);
            // hudWeaponSlot.SetCurrentSlotItem(hud_Weapon_Items[currIndex], currIndex);
        }
    }

    void Down()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (currIndex == items.Count - 1) return;
            currIndex++;
            // content.anchoredPosition = new Vector2(content.anchoredPosition.x, -height * currIndex);
            ScrollToWeapon(currIndex);
            hudWeaponSlot.HandleWeaponSlotItem(currIndex);

        }
    }

    public void ScrollToWeapon(int index, bool animate = true)
    {
        if (animate)
        {
            StartCoroutine(MoveFromTo(content.offsetMax, new Vector2(content.anchoredPosition.x, height * index), scrollSpeed, content));
            // StartCoroutine(MoveFromTo(content.offsetMax, new Vector2(content.offsetMax.x, height * index), scrollSpeed, content));
        }
        else
        {
            content.offsetMax = new Vector2(content.anchoredPosition.x, height * currIndex);
        }
        hudWeaponSlot.SetCurrentSlotItem(hud_Weapon_Items[index]);
    }

    IEnumerator MoveFromTo(Vector2 from, Vector2 to, float speed, RectTransform tra)
    {
        var t = 0f;
        while (t < 1f)
        {
            t += speed * Time.deltaTime;
            tra.offsetMax = Vector3.Lerp(from, to, t);
            yield return null;
        }
    }

    // RectTransform rectTransform;
    // /*Left*/ rectTransform.offsetMin.x;
    // /*Right*/ rectTransform.offsetMax.x;
    // /*Top*/ rectTransform.offsetMax.y;
    // /*Bottom*/ rectTransform.offsetMin.y;
}
