using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class TextInputController : MonoBehaviour
{
    OilPaintEngine OilPaintEngine;
    TMP_InputField InputField;

    // Start is called before the first frame update
    void Start()
    {
        OilPaintEngine = GameObject.Find("OilPaintEngine").GetComponent<OilPaintEngine>();
        InputField = GetComponent<TMP_InputField>();
        InputField.onValueChanged.AddListener(OnValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged(string arg0)
    {
        int value = int.Parse(arg0);
        Vector2 normal = MathUtil.RotateAroundOrigin(Vector2Int.right, value);
        OilPaintEngine.UpdateRakelNormal(normal);
    }
}
