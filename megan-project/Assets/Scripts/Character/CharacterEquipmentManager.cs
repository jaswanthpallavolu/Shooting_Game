using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipmentManager : MonoBehaviour
{
    public List<AnimStateList> animStateList;
    protected virtual void Awake()
    {
        animStateList = new List<AnimStateList>(){
            new AnimStateList {AnimState = WeaponAnimState.HG, AnimStateId = Animator.StringToHash("HG")},
            new AnimStateList {AnimState = WeaponAnimState.AR, AnimStateId = Animator.StringToHash("AR")}
        };
    }
    protected virtual void Start()
    {

    }

}

public class AnimStateList
{
    public WeaponAnimState AnimState { set; get; }
    public int AnimStateId { set; get; }
}
