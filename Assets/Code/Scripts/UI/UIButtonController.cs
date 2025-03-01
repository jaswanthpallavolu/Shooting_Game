using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonController : MonoBehaviour
{

    [SerializeField] float holdTime = 5f;
    [SerializeField] float currTime = 0f;
    [SerializeField] float fillAmount;
    [SerializeField] float fillSpeed = 5f;

    ButtonPromptData buttonData;

    [Header("Button")]
    [SerializeField] RectTransform textMesh;
    [SerializeField] RectTransform fillRect;
    [SerializeField] RectTransform keyRect;

    public event EventHandler<UIButtonController> OnHoldComplete;

    void Start()
    {
        ResetState();
    }

    void Update()
    {
        PressAndHold();
    }

    void PressAndHold()
    {
        if (Input.GetKey(KeyCode.V))
        {
            currTime += Time.deltaTime;
            fillAmount = currTime / holdTime;
            FillAmount(fillAmount);

            if (currTime >= holdTime)
            {
                if (OnHoldComplete != null && fillRect.GetComponent<Image>().fillAmount == 1)
                    OnHoldComplete(this, GetComponent<UIButtonController>());
            }
        }
        else
        {
            fillAmount = 0;
            currTime = 0;
            FillAmount(fillAmount);
        }
    }

    public void SetButtonData(ButtonPromptData data)
    {
        buttonData = data;
        textMesh.GetComponent<TextMeshProUGUI>().text = buttonData.Text;
        keyRect.GetComponent<Image>().sprite = buttonData.KeySprite;
    }

    private void FillAmount(float fillAmount)
    {
        fillRect.GetComponent<Image>().fillAmount = Mathf.Lerp(fillRect.GetComponent<Image>().fillAmount, fillAmount, Time.deltaTime * fillSpeed);
    }

    public void ResetState()
    {
        fillRect.GetComponent<Image>().fillAmount = 0;
    }
}
