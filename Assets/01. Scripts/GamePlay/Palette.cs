using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Palette : MonoBehaviour
{
    private PaletteColor[] _paletteColors;
    private Board _board;
    
    public PaletteColor[] PaletteColors => _paletteColors;


    private void Awake()
    {
        _paletteColors = GetComponentsInChildren<PaletteColor>();
    }
    
    private void Start()
    {
        _board = FindAnyObjectByType<Board>();

        UpdatePaletteVisibility();
    }
    
    private void UpdatePaletteVisibility()
    {
        //활성화 여부를 판단하는 불변수를 가지고 있는 딕셔너리
         Dictionary<CellColor, bool> colorAvailability;
         colorAvailability = new Dictionary<CellColor, bool>
         {
             { CellColor.Blue, false },
             { CellColor.Red, false },
             { CellColor.Yellow, false },
             { CellColor.Green, false }
         };
        
        //foreach문으로 쉽게 조회할 수 있게 변경
        var allCells = Enumerable.Range(0, _board.Cells.Length)
            .Select(i => _board.Cells[i / Board.Cols, i % Board.Cols]);

        //판에 존재하는 색 판단
        foreach (var cell in allCells)
        {
            if (colorAvailability.ContainsKey(cell.Color))
            {
                colorAvailability[cell.Color] = true;
            }
        }
        
        //판에 없는 색 비활성화
        foreach (var paletteColor in _paletteColors)
        {
            bool isAvailable = colorAvailability.GetValueOrDefault(paletteColor.Color, false);
            paletteColor.gameObject.SetActive(isAvailable);
        }
    }
    
    public void UpdatePalette()
    {
        foreach (PaletteColor color in _paletteColors)
        {
            if (color.gameObject.activeSelf)
            {
                color.SetOutLine(false);
            }
        }
    }
    

    

}