using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] RectTransform fillRect;
    [SerializeField] RectTransform keyRect;
    [SerializeField] Sprite Key_InputSprite;
    [SerializeField] Sprite XB_InputSprite;
    [SerializeField] Sprite PS_InputSprite;

    // Start is called before the first frame update
    void Start()
    {
        SetImage();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetImage()
    {
        keyRect.GetComponent<Image>().sprite = Key_InputSprite;
    }

    public void FillAmount(float fillAmount)
    {
        fillRect.GetComponent<Image>().fillAmount = Mathf.Lerp(fillRect.GetComponent<Image>().fillAmount, fillAmount, Time.deltaTime * 5f);
    }
}
