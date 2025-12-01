using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevStage : MonoBehaviour
{
    private CellColor _selectedColor;
    private Palette _palette;
    private Board _board;
    private bool _isHovered;
    private SetTargetColorButton _setTargetColorButton;
    private TargetColorText _targetColorText;
    private Dictionary<CellColor,Action> _targetColorActions;   //기능 삭제를 위해 추가
    private PopUpTexts _popUpTexts;
    private CompleteButton _completeButton;
    private PopUpList _popUpList;
    private Canvas _canvas;
    private ResetButton _resetButton;

    private void Awake()
    {
        _popUpTexts = GetComponent<PopUpTexts>();
        _popUpList = GetComponent<PopUpList>();
    }

    private void Start()
    {
        _palette = FindAnyObjectByType<Palette>();
        _board = FindAnyObjectByType<Board>();
        _setTargetColorButton = FindAnyObjectByType<SetTargetColorButton>();
        _targetColorText = FindAnyObjectByType<TargetColorText>();
        _completeButton = FindAnyObjectByType<CompleteButton>();
        _canvas = FindAnyObjectByType<Canvas>();
        _resetButton = FindAnyObjectByType<ResetButton>();
        
        
        RegisterSetTargetColorButton();
        RegisterSelectedColor();
        RegisterCell();
        RegisterCompleteButton();
        RegisterResetButton();
    }
    private void OnDestroy()
    {
        ResetSelectedColor();
        ResetCell();
        ResetSetTargetColorButton();
        ResetCompleteButton();
        ResetResetButton();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            _isHovered = true;
        if (Input.GetMouseButtonUp(0))
            _isHovered = false;
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
        _setTargetColorButton.Button.onClick.AddListener(() => SetTargetColorAlert());
    }

    private void SetTargetColorAlert()
    {
        // 1. 오브젝트 생성
        GameObject go = ObjectPool.Get(PoolIndex.Alert, _popUpList.AlertPopUp);
        
        // 2. 생성한 오브젝트는 UI이기 때문에 캔버스의 자식으로 설정
        go.transform.SetParent(_canvas.transform);
        
        // 3. 위치 초기화
        go.transform.localPosition = Vector3.zero;
        
        // 4. 생성된 오브젝트에서 원하는 컴포넌트를 가져오기
        if (go.TryGetComponent(out AlertPopUp alertPopUp))
        {
            // 5. 알림창 문구 수정
            alertPopUp.SetDescription(_popUpTexts.TargetColorText);
        }
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

    #region CompleteButton

    private void RegisterCompleteButton()
    {
        _completeButton.Button.onClick.AddListener(Complete);
    }

    private void Complete()
    {
        var solver = new BFSSolver();
 
        if (solver.TrySolve(_board.CurrentCells, _targetColorText.Color, out int minimumMoves) && 
            _targetColorText.Color != CellColor.None)
        {
            GameObject go = ObjectPool.Get(PoolIndex.Confirm, _popUpList.ConfirmPopUp);
            go.transform.SetParent(_canvas.transform);
            go.transform.localPosition = Vector3.zero;
            if (go.TryGetComponent(out ConfirmPopUp _confirmPopUp))
            {
                string text = String.Format(_popUpTexts.CompleteText, minimumMoves);
                _confirmPopUp.SetDescription(text);
                
                //팝업 창 버튼에 기능 추가
                _confirmPopUp.YesButton.onClick.AddListener(() => SceneMng.ChangeScene(SceneName.LobbyScene));
                StageData saveData = new StageData(minimumMoves, _targetColorText.Color, _board.CurrentCells);
                _confirmPopUp.YesButton.onClick.AddListener(() => StageSaveLoader.SaveStage(saveData));
            }
        }   
        //계산 실패 시 알람
        else
        {
            GameObject go = ObjectPool.Get(PoolIndex.Alert, _popUpList.AlertPopUp);
            go.transform.SetParent(_canvas.transform);
            go.transform.localPosition = Vector3.zero;
            if (go.TryGetComponent(out AlertPopUp alertPopUp))
            {
                alertPopUp.SetDescription(_popUpTexts.FailText);
            }
        }
    }

    private void ResetCompleteButton()
    {
        _completeButton.Button.onClick.RemoveAllListeners();   
    }

    #endregion

    #region resetButton

    private void RegisterResetButton()
    {
        _resetButton.OnReset += () => _board.ResetBoard();
    }
    private void ResetResetButton()
    {
        _resetButton.OnReset = null;
    }
    
    #endregion
}
