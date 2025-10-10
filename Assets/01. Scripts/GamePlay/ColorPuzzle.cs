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
    private LimitedChances _limitedChances;
    private ResetButton _resetButton;
    private PopUpUI _popUpUI;
    
    [SerializeField] private CellColor targetColor;
    
    private void Start()
    {
        _paletteColors = FindObjectsOfType<PaletteColor>();
        _limitedChances = FindAnyObjectByType<LimitedChances>();
        _resetButton = FindAnyObjectByType<ResetButton>();
        _board = FindAnyObjectByType<Board>();
        _popUpUI = FindAnyObjectByType<PopUpUI>();
        
        RegiseterSelectedColor();
        RegisterCell();
        RegisterResetButton();
        RegisterPopUp();
    }

    private void OnDestroy()
    {
        ResetSelectedColor();
        ResetCell();
        ResetResetButton();
        ResetPopUp();
    }

    private void RegiseterSelectedColor()
    {

        foreach (var paletteColor in _paletteColors)
        {
            paletteColor.OnColorSelected += () => _selectedColor = paletteColor.Color;
        }
    }

    private void ResetSelectedColor()
    {
        foreach (var paletteColor in _paletteColors)
        {
            paletteColor.OnColorSelected = null;
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

    private void CountingChances(CellColor prevColor)
    {
        //색이 선택되지 않았거나, 선택된 색이 같은 때 작동하지 않음.
        if(_selectedColor == CellColor.None || _selectedColor == prevColor)
            return;
        
        _limitedChances.UsingChances();
    }

    private void RegisterCell()
    {

        for (int i = 0; i < Board.Rows; i++)
        {
            for (int j = 0; j < Board.Cols; j++)
            {
                int x = i;
                int y = j;
                _board.Cells[i,j].OnCellClicked += () => CountingChances(_board.Cells[x,y].Color);
                _board.Cells[i,j].OnCellClicked += () => TrySolve(x,y,_board.Cells[x,y].Color);
                _board.Cells[i,j].OnCellClicked += () => CheckClear();
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
    
    private void RegisterResetButton()
    {
        _resetButton.OnReset += () => _board.ResetBoard();
        _resetButton.OnReset += () => _limitedChances.ResetChances();
    }
    
    private void ResetResetButton()
    {
        _resetButton.OnReset = null;
    }
    
    private void RegisterPopUp()
    {
        _popUpUI.OnRetry += () => _board.ResetBoard();
        _popUpUI.OnRetry += () => _limitedChances.ResetChances();
        _popUpUI.OnRetry += () => _popUpUI.gameObject.SetActive(false);
        
        _popUpUI.gameObject.SetActive(false);
    }
    
    private void ResetPopUp()
    {
        _popUpUI.OnRetry = null;
    }

    private void CheckClear()
    {
        bool clear = true;
        
        for(int i=0;i<Board.Rows;i++)
        {
            for(int j=0;j<Board.Cols;j++)
            {
                if(_board.Cells[i,j].Color != targetColor)
                    clear = false;
            }
        }
        
        if(clear)
            ClearPopUp();
        else if(_limitedChances.Chances == 0)
            FailPopUp();
    }

    private void ClearPopUp()
    {
        _popUpUI.gameObject.SetActive(true);
        _popUpUI.ClearPopUp();
    }
    
    private void FailPopUp()
    {
        _popUpUI.gameObject.SetActive(true);
        _popUpUI.FailPopUp();
    }
}
