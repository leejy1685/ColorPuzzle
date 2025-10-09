using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPuzzle : MonoBehaviour
{
    private CellColor _selectedColor;
    private PaletteColor[] _paletteColors;
    
    private void Start()
    {
        RegiseterSelectedColor();
    }

    private void OnDestroy()
    {
        ResetSelectedColor();
    }

    private void RegiseterSelectedColor()
    {
        _paletteColors = FindObjectsOfType<PaletteColor>();

        foreach (var paletteColor in _paletteColors)
        {
            paletteColor.OnColorSelected += () => _selectedColor = paletteColor.Color;
        }
    }

    private void ResetSelectedColor()
    {
        foreach (var paletteColor in _paletteColors)
        {
            paletteColor.OnColorSelected -= () => _selectedColor = paletteColor.Color;
        }
    }
    
    
    
    
}
