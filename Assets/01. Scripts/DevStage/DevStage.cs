using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class DevStage : MonoBehaviour
{
    private CellColor _selectedColor;
    private Palette _palette;
    private Board _board;
    private AlertPopUp _alertPopUp;
    private bool _isHovered;
    private SetTargetColorButton _setTargetColorButton;
    private TargetColorText _targetColorText;
    private Dictionary<CellColor,Action> _targetColorActions;   //기능 삭제를 위해 추가
    private ConfirmPopUp _confirmPopUp;
    private PopUpTexts _popUpTexts;

    private void Start()
    {
        _palette = FindAnyObjectByType<Palette>();
        _board = FindAnyObjectByType<Board>();
        _alertPopUp = FindAnyObjectByType<AlertPopUp>();
        _setTargetColorButton = FindAnyObjectByType<SetTargetColorButton>();
        _targetColorText = FindAnyObjectByType<TargetColorText>();
        _confirmPopUp = FindAnyObjectByType<ConfirmPopUp>();
        _popUpTexts = GetComponent<PopUpTexts>();
        
        
        RegisterSetTargetColorButton();
        RegisterPopUp();
        RegisterSelectedColor();
        RegisterCell();

        //Test Code
        // var solver = new BFSSolver();
        //
        // if (solver.TrySolve(_board.FirstCells, CellColor.Red, out int minimumMoves))
        // {
        //     Debug.Log($"✅ 현재 보드의 최저 횟수는 {minimumMoves}회 입니다.");
        // }
        // else
        // {
        //     Debug.Log("❌ 현재 보드 상태로는 목표를 달성할 수 없습니다.");
        // }
        //
        // StageData saveData = new StageData(minimumMoves, CellColor.Red, _board.FirstCells);
        // StageSaveLoader.SaveStage(saveData);
        
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
        ResetSetTargetColorButton();
        ResetPopUp();
    }

    #region pallette
    
    //팔렛트 색 선택 기능 추가
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

    //팔렛트 색 선택 기능 삭제
    private void ResetSelectedColor()
    {
        foreach (var paletteColor in _palette.PaletteColors)
        {
            paletteColor.OnColorSelected = null;
        }
    }
    
    #endregion

    #region Cell

    //셀 클릭 기능 추가
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

    //PointEnter를 위한 체인지 컬러
    private void ChangeColor(int x, int y, CellColor color)
    {
        if(_isHovered) 
            _board.Cells[x,y].ChangeColor(color);
    }
    
    //셀 클릭 기능 삭제
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

    #endregion

    #region SetTargetColorButton

    //타겟 컬러 설정 버튼 기능 추가
    private void RegisterSetTargetColorButton()
    {
        _setTargetColorButton.Button.onClick.AddListener(OnClickSetTargetColorButton);
        _setTargetColorButton.Button.onClick.AddListener(() => _alertPopUp.gameObject.SetActive(true));
        _setTargetColorButton.Button.onClick.AddListener(() => _alertPopUp.SetDescription(_popUpTexts.TargetColorText));
    }
    
    //타겟 컬러 설정 버튼 기능 삭제
    private void ResetSetTargetColorButton()
    {
        _setTargetColorButton.Button.onClick.RemoveAllListeners();
    }

    //타겟 컬러 설정 버튼 기능(팔렛트 색 선택 시 타겟 컬러 설정)
    private void OnClickSetTargetColorButton()
    {
        _targetColorActions = new Dictionary<CellColor, Action>();
        
        foreach (var paletteColor in _palette.PaletteColors)
        {
            _targetColorActions.Add(paletteColor.Color,
                () => OnSelectSetTargetColor(paletteColor.Color));
            paletteColor.OnColorSelected += _targetColorActions[paletteColor.Color];
        }
    }
    
    //타겟 컬러 설정 후, 팔렛트에 기능 삭제
    private void OnSelectSetTargetColor(CellColor color)
    {
        _targetColorText.SetTargetColor(color);
        
        foreach (var paletteColor in _palette.PaletteColors)
        {
            paletteColor.OnColorSelected -= _targetColorActions[paletteColor.Color];
        }
    }
    
    #endregion
    
    #region PopUp
    //알람 팝업 기능 등록
    private void RegisterPopUp()
    {
        _alertPopUp.OkButton.onClick.AddListener(() => _alertPopUp.gameObject.SetActive(false));
        _confirmPopUp.YesButton.onClick.AddListener(() => _confirmPopUp.gameObject.SetActive(false));
        _confirmPopUp.NoButton.onClick.AddListener(() => _confirmPopUp.gameObject.SetActive(false));
        
        _alertPopUp.gameObject.SetActive(false);
        _confirmPopUp.gameObject.SetActive(false);
    }

    //알람 팝업 기능 삭제
    private void ResetPopUp()
    {
        _alertPopUp.OkButton.onClick.RemoveAllListeners();
        _confirmPopUp.YesButton.onClick.RemoveAllListeners();
        _confirmPopUp.NoButton.onClick.RemoveAllListeners();
    }
    
    
    #endregion
}
