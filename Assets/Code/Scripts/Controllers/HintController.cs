using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintController : Controller
{
    private string _button;
    private string _buttonFunction;
    private GameObject hintObject;
    private TextMeshProUGUI textMeshProUGUI;
    private string parsedContent = "";

    void Start()
    {
        hintObject = findChild("Hint");
        textMeshProUGUI = hintObject.GetComponent<TextMeshProUGUI>();
    }

    public void setHintRaw(string raw)
    {
        textMeshProUGUI.text = raw;
    }

    public void setHint(string button, string text)
    {
        string content = parseContent(button, text);
        textMeshProUGUI.text = content;
    }

    string parseContent(string button, string buttonFunction)
    {
        return "<sprite name=\"" + button + "\"> " + buttonFunction;
    }

    public void setActive(bool state)
    {
        hintObject.SetActive(state);
    }

    public bool isVisible {
        get { return hintObject.activeSelf; }
    }
}
