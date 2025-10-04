using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{
    private PaletteColor[] _colors;
    
    private void Awake()
    {
        _colors = GetComponentsInChildren<PaletteColor>();
        
        //판의 색을 전체 조회해서 없는 색은 비활성화
        //ex)
        //if(_colors[0].Color == CellColor.Blue)
        //    _colors[0].gameObject.SetActive(false);
    }
    
    
}
