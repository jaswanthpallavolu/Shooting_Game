using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud_Weapon_Item : MonoBehaviour
{
    [SerializeField] public WeaponItemData weaponItemData;
    [SerializeField] private Image ItemImage;

    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetItemImage(Sprite sprite)
    {
        ItemImage.GetComponent<Image>().sprite = sprite;
    }

}
