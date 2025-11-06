using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevStage : MonoBehaviour
{
    private CellColor _selectedColor;
    private Palette _palette;
    private Board _board;
    private bool _isHovered;

    private void Start()
    {
        _palette = FindAnyObjectByType<Palette>();
        _board = FindAnyObjectByType<Board>();
        
        RegisterSelectedColor();
        RegisterCell();
        
        
        //Test Code
        var solver = new BFSSolver();

        if (solver.TrySolve(_board.FirstCells, CellColor.Red, out int minimumMoves))
        {
            Debug.Log($"✅ 현재 보드의 최저 횟수는 {minimumMoves}회 입니다.");
        }
        else
        {
            Debug.Log("❌ 현재 보드 상태로는 목표를 달성할 수 없습니다.");
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            _isHovered = true;
        if (Input.GetMouseButtonUp(0))
            _isHovered = false;
    }

    private void OnDestroy()
    {
        ResetSelectedColor();
        ResetCell();
    }
    
    private void RegisterSelectedColor()
    {
        foreach (var paletteColor in _palette.PaletteColors)
        {
            paletteColor.OnColorSelected += () => _palette.UpdatePalette();             //팔레트 초기화
            paletteColor.OnColorSelected += () => paletteColor.SetOutLine(true);    //선택 표시
            paletteColor.OnColorSelected += () => _selectedColor = paletteColor.Color;  //선택
        }
        
        _palette.SelectedFirstColor();
    }

    private void ResetSelectedColor()
    {
        foreach (var paletteColor in _palette.PaletteColors)
        {
            paletteColor.OnColorSelected = null;
        }
    }

    private void RegisterCell()
    {
        for (int i = 0; i < Board.Rows; i++)
        {
            for (int j = 0; j < Board.Cols; j++)
            {
                int x = i;
                int y = j;
                _board.Cells[i,j].OnCellClicked += () => _board.Cells[x,y].ChangeColor(_selectedColor);
                _board.Cells[i,j].OnCellHovered += () => ChangeColor(x,y,_selectedColor);
            }
        }
    }

    private void ChangeColor(int x, int y, CellColor color)
    {
        if(_isHovered) 
            _board.Cells[x,y].ChangeColor(color);
    }
    
    private void ResetCell()
    {
        for (int i = 0; i < Board.Rows; i++)
        {
            for (int j = 0; j < Board.Cols; j++)
            {
                _board.Cells[i, j].OnCellClicked = null;
                _board.Cells[i, j].OnCellHovered = null;
            }
        }
    }
}
