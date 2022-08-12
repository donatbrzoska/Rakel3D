using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]
public class RakelPaintApplyButtonController : MonoBehaviour
{
    OilPaintEngine OilPaintEngine;
    Button Button;

    TMP_Dropdown ColorDropdown;
    TMP_InputField VolumeInputField;

    Dictionary<string, Color> ColorMapper = new Dictionary<string, Color>()
    {
        { "Anthracite", new Color(0.25f, 0.25f, 0.25f) },
        { "Red", new Color(0.8f, 0.08f, 0.03f) },
        { "Dark Red", new Color(0.58f, 0.06f, 0f) },
        { "Green", new Color(0.02f, 0.57f, 0.04f) },
        { "Blue", new Color(0.12f, 0.49f, 0.93f) },
        { "Dark Blue", new Color(0.05f, 0.12f, 0.32f) },
        { "Yellow", new Color(1f, 0.88f, 0.12f) },
        { "Orange", new Color(1f, 0.64f, 0.06f) },
        { "Purple", new Color(0.5f, 0.3f, 0.99f) },
        { "White", new Color(0.99f, 0.99f, 0.99f) },
        { "Black", new Color(0.01f, 0.01f, 0.01f) },
        { "Red Red", new Color(1f, 0f, 0f) },
        { "Green Green", new Color(0f, 1f, 0f) },
        { "Blue Blue", new Color(0f, 0f, 1f) },
    };

    public void Awake()
    {
        OilPaintEngine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);

        ColorDropdown = GameObject.Find("Predefined Colors Dropdown").GetComponent<TMP_Dropdown>();
        VolumeInputField = GameObject.Find("Paint Volume Value").GetComponent<TMP_InputField>();
    }

    public void Start()
    {
        ColorDropdown.SetValueWithoutNotify(0); // TODO retrieve from OilPaintEngine? Requires backwards mapping though ...
        VolumeInputField.SetTextWithoutNotify("" + OilPaintEngine.RakelPaintVolume);
    }

    public void OnClick()
    {
        Color color = ColorMapper[ColorDropdown.options[ColorDropdown.value].text];
        int volume = int.Parse(VolumeInputField.text);

        OilPaintEngine.UpdateRakelPaint(color, volume);
    }
}
