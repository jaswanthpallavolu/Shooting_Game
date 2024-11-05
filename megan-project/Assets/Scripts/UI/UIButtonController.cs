using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIButtonController : MonoBehaviour
{
    [SerializeField] UIButton button;
    [SerializeField] RectTransform textMesh;
    [SerializeField] string text;

    [SerializeField] float holdTime = 5f;
    [SerializeField] float currTime = 0f;

    [SerializeField] float fillAmount;

    // Start is called before the first frame update
    void Start()
    {
        textMesh.GetComponent<TextMeshProUGUI>().text = text;
    }

    // Update is called once per frame
    void Update()
    {
        // textMesh.GetComponent<TextMeshProUGUI>().text = text;
        if (Input.GetKey(KeyCode.V))
        {
            currTime += Time.deltaTime;
            fillAmount = currTime / holdTime;
            button.FillAmount(fillAmount);

            if (currTime >= holdTime)
            {
                Debug.Log("perform action");
            }
        }
        else
        {
            fillAmount = 0;
            currTime = 0;
            button.FillAmount(fillAmount);
        }



    }
}
