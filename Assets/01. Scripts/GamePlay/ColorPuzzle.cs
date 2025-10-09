using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorPuzzle : MonoBehaviour
{
    private CellColor _selectedColor;
    private PaletteColor[] _paletteColors;
    private Board _board;
    private int[,] _direction = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };   //상하좌우
    
    private void Start()
    {
        RegiseterSelectedColor();
        RegisterCell();
    }

    private void OnDestroy()
    {
        ResetSelectedColor();
        ResetCell();
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
    
    private void TrySolve(int x,int y,CellColor prevColor)
    {
        //색이 선택되지 않았거나, 선택된 색이 같은 때 작동하지 않음.
        if(_selectedColor == CellColor.None || _selectedColor == prevColor)
            return;
        
        _board.Cells[x,y].ChangeColor(_selectedColor);
        

        for (int i = 0; i < _direction.GetLength(0); i++)
        {
            int nx = x + _direction[i, 0];
            int ny = y + _direction[i, 1];
            
            if (nx < 0 || nx >= Board.Rows || ny < 0 || ny >= Board.Cols)
                continue;
            
            if (_board.Cells[nx, ny].Color == prevColor)
                TrySolve(nx, ny, prevColor);
        }
    }

    private void RegisterCell()
    {
        _board = FindAnyObjectByType<Board>();

        for (int i = 0; i < Board.Rows; i++)
        {
            for (int j = 0; j < Board.Cols; j++)
            {
                int x = i;
                int y = j;
                _board.Cells[i,j].OnCellClicked += () => TrySolve(x,y,_board.Cells[x,y].Color);
            }
        }
    }

    private void ResetCell()
    {
        for (int i = 0; i < Board.Rows; i++)
        {
            for (int j = 0; j < Board.Cols; j++)
            {
                _board.Cells[i, j].OnCellClicked = null;
            }
        }
    }
    

    
    
    
    
}
