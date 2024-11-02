using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class HUDWeaponSlideController : MonoBehaviour
{
    // ScrollView scrollView;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] RectTransform contentPanel;
    [SerializeField] Hud_Weapon_Item[] hud_Weapon_Items;
    [SerializeField] int scrollToIndex = 0;
    [SerializeField] bool select = false;
    [SerializeField] float scrollSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        // scrollView = GetComponent<ScrollView>();
        scrollRect = GetComponent<ScrollRect>();
        contentPanel = scrollRect.content;
        scrollRect.onValueChanged.AddListener(OnItemChange);

    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (hud_Weapon_Items.Length == 0)
    //         hud_Weapon_Items = contentPanel.GetComponentsInChildren<Hud_Weapon_Item>();
    //     // Debug.Log(scrollView.scrollOffset);
    //     // Debug.Log(scrollRect.content);
    //     // if (select)
    //     // {
    //     //     select = false;
    //     //     SnapTo(hud_Weapon_Items[scrollToIndex].GetComponent<RectTransform>());
    //     // }
    //     scrollRect.viewport.GetComponent<Mask>().enabled = false;
    // }

    // Sets the speed to move the scrollbar
    // public float scrollSpeed = 10f;

    // Set as Template Object via (Your Dropdown Button > Template)
    public ScrollRect m_templateScrollRect;

    // Set as Template Viewport Object via (Your Dropdown Button > Template > Viewport)
    public RectTransform m_templateViewportTransform;

    // Set as Template Content Object via (Your Dropdown Button > Template > Viewport > Content)
    public RectTransform m_ContentRectTransform;

    public RectTransform m_SelectedRectTransform;

    void Update()
    {
        hud_Weapon_Items = contentPanel.GetComponentsInChildren<Hud_Weapon_Item>();
        if (select)
        {
            // select = false;
            UpdateScrollToSelected(m_templateScrollRect, m_ContentRectTransform, m_templateViewportTransform);
        }
    }

    void UpdateScrollToSelected(ScrollRect scrollRect, RectTransform contentRectTransform, RectTransform viewportRectTransform)
    {
        // Get the current selected option from the eventsystem.
        // GameObject selected = EventSystem.current.currentSelectedGameObject;
        GameObject selected = hud_Weapon_Items[scrollToIndex].gameObject;

        if (selected == null)
        {
            return;
        }
        if (selected.transform.parent != contentRectTransform.transform)
        {
            return;
        }

        m_SelectedRectTransform = selected.GetComponent<RectTransform>();

        // Math stuff
        Vector3 selectedDifference = viewportRectTransform.localPosition - m_SelectedRectTransform.localPosition;
        float contentHeightDifference = contentRectTransform.rect.height - viewportRectTransform.rect.height;

        float selectedPosition = contentRectTransform.rect.height - selectedDifference.y;
        float currentScrollRectPosition = scrollRect.normalizedPosition.y * contentHeightDifference;
        float above = currentScrollRectPosition - (m_SelectedRectTransform.rect.height / 2) + viewportRectTransform.rect.height;
        float below = currentScrollRectPosition + (m_SelectedRectTransform.rect.height / 2);

        Debug.Log(selectedPosition + "," + above + "," + below);
        // Check if selected option is out of bounds.
        if (selectedPosition > above)
        {
            float step = selectedPosition - above;
            float newY = currentScrollRectPosition + step;
            float newNormalizedY = newY / contentHeightDifference;
            Debug.Log("newNormalizedY " + newNormalizedY);
            scrollRect.normalizedPosition = Vector2.Lerp(scrollRect.normalizedPosition, new Vector2(0, newNormalizedY), scrollSpeed * Time.deltaTime);
            // scrollRect.normalizedPosition = new Vector2(0, newNormalizedY);
        }
        else if (selectedPosition < below)
        {
            float step = selectedPosition - below;
            float newY = currentScrollRectPosition + step;
            float newNormalizedY = newY / contentHeightDifference;
            Debug.Log("newNormalizedY " + newNormalizedY);
            scrollRect.normalizedPosition = Vector2.Lerp(scrollRect.normalizedPosition, new Vector2(0, newNormalizedY), scrollSpeed * Time.deltaTime);
            // scrollRect.normalizedPosition = new Vector2(0, newNormalizedY);
        }
    }

    public void OnItemChange(Vector2 value)
    {
        Debug.Log("ListenerMethod: " + value);
    }



    // public void SnapTo(RectTransform target)
    // {
    //     Canvas.ForceUpdateCanvases();
    //     contentPanel.anchoredPosition =
    //             (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
    //             - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    // }
}
