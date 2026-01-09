using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetColorText : MonoBehaviour
{
    private TextMeshProUGUI _text; 
    private CellColor _color;
    
    public CellColor Color => _color;
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }
    
    public void SetTargetColor(CellColor color)
    {
        _color = color;
        _text.text = color.ToString();
        _text.color = ColorConverter.ColorCodeToColor(color);
    }
}
