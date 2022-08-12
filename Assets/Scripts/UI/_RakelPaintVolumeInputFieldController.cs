using System;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class RakelPaintVolumeInputFieldController : InputFieldController
{
    RakelPaintController rpc = GameObject.Find("Paint Color Text").GetComponent<RakelPaintController>();

    public void Start()
    {
        InputField.text = "" + OilPaintEngine.RakelPaintVolume;
    }

    override public void OnValueChanged(string arg0)
    {
        int value = int.Parse(arg0);
        rpc.Volume = value;
    }
}