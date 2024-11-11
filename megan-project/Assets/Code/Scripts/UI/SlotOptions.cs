using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotOptions : MonoBehaviour
{
    [SerializeField] GameObject buttonHold;
    [SerializeField] string helperText;
    [SerializeField] Sprite KeyInputSprite;
    [SerializeField] Sprite XBInputSprite;
    [SerializeField] Sprite PSInputSprite;

    public UIButtonController swapButton;

    // List<ButtonPromptData> buttonPrompts = new List<ButtonPromptData>();



    void Awake()
    {
        ButtonPromptData swapBPD = new ButtonPromptData();
        swapBPD.Text = helperText;
        swapBPD.KeySprite = KeyInputSprite;
        // swapBPD.XBSprite = XBInputSprite;
        // swapBPD.PSSprite = PSInputSprite;
        // buttonPrompts.Add(swapBPD);

        GameObject spawnedButton = Instantiate(buttonHold, transform);
        swapButton = spawnedButton.GetComponent<UIButtonController>();
        swapButton.SetButtonData(swapBPD);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public struct ButtonPromptData
{
    public string Text { get; set; }
    public Sprite KeySprite { get; set; }
    public Sprite XBSprite { get; set; }
    public Sprite PSSprite { get; set; }
}
