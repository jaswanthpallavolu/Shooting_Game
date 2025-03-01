using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewNavigation : MonoBehaviour
{
    public int CurrIndex { get; set; }
    public int TotalCount { get; set; }

    [SerializeField] RectTransform arrowUp;
    [SerializeField] RectTransform arrowDown;
    [SerializeField] RectTransform indicator;
    [SerializeField] RectTransform dotIndicator;
    List<RectTransform> dots = new List<RectTransform>();

    void Start()
    {

    }

    void Update()
    {
        if (dots.Count == 0)
        {
            for (int i = 0; i < TotalCount; i++)
            {
                var spawnedDot = Instantiate(dotIndicator, indicator.transform);
                dots.Add(spawnedDot);
            }
        }

        if (TotalCount == 1)
        {
            arrowUp.gameObject.SetActive(false);
            arrowDown.gameObject.SetActive(false);
            indicator.gameObject.SetActive(false);
        }
        else
        {
            arrowUp.gameObject.SetActive(true);
            arrowDown.gameObject.SetActive(true);
            indicator.gameObject.SetActive(true);
        }

        if (CurrIndex == 0)
        {
            arrowUp.gameObject.SetActive(false);
            arrowDown.gameObject.SetActive(true);
        }
        else if (CurrIndex == TotalCount - 1)
        {
            arrowUp.gameObject.SetActive(true);
            arrowDown.gameObject.SetActive(false);
        }
        else
        {
            arrowUp.gameObject.SetActive(true);
            arrowDown.gameObject.SetActive(true);
        }

        for (int i = 0; i < TotalCount; i++)
        {
            if (i == CurrIndex) dots[i].localScale = new Vector3(2, 2, 2);
            else dots[i].localScale = new Vector3(1, 1, 1);
        }
    }
}
