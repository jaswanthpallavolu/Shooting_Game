using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] RectTransform fillRect;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FillAmount(float fillAmount)
    {
        fillRect.GetComponent<Image>().fillAmount = Mathf.Lerp(fillRect.GetComponent<Image>().fillAmount, fillAmount, Time.deltaTime * 5f);
    }
}
