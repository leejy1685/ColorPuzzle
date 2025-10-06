using System;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private CellColor color;
    
    private Image _image;
    
    public CellColor Color => color;
    
    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.color = ColorConverter.ColorCodeToColor(color);
    }

    public void ChangeColor(CellColor newColor)
    {
        color = newColor;
        _image.color = ColorConverter.ColorCodeToColor(newColor);
    }


}
