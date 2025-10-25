using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetColorText : MonoBehaviour
{
    private TextMeshProUGUI _text; 
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }
    
    public void SetTargetColor(CellColor color)
    {
        _text.text = color.ToString();
        _text.color = ColorConverter.ColorCodeToColor(color);
    }
}
