using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Palette : MonoBehaviour
{
    private PaletteColor[] _paletteColors;
    private Board _board;
    private Dictionary<CellColor, bool> _colorAvailability;

    private void Awake()
    {
        _paletteColors = GetComponentsInChildren<PaletteColor>();
        _colorAvailability = new Dictionary<CellColor, bool>
        {
            { CellColor.Blue, false },
            { CellColor.Red, false },
            { CellColor.Yellow, false },
            { CellColor.Green, false }
        };
    }
    
    private void Start()
    {
        _board = FindAnyObjectByType<Board>();

        UpdatePaletteVisibility();
    }
    
    
    
    private void UpdatePaletteVisibility()
    {
        //foreach문으로 쉽게 조회할 수 있게 변경
        var allCells = Enumerable.Range(0, _board.Cells.Length)
            .Select(i => _board.Cells[i / Board.Cols, i % Board.Cols]);

        //판에 존재하는 색 판단
        foreach (var cell in allCells)
        {
            if (_colorAvailability.ContainsKey(cell.Color))
            {
                _colorAvailability[cell.Color] = true;
            }
        }
        
        //판에 없는 색 비활성화
        foreach (var paletteColor in _paletteColors)
        {
            bool isAvailable = _colorAvailability.GetValueOrDefault(paletteColor.Color, false);
            paletteColor.gameObject.SetActive(isAvailable);
        }
    }
}