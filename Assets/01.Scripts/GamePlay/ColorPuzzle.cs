using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorPuzzle : MonoBehaviour
{
    private CellColor _selectedColor;
    private Palette _palette;
    private Board _board;
    private LimitedChances _limitedChances;
    private ResetButton _resetButton;
    private TargetColorText _targetColorText;
    private PopUpTexts _popUpTexts;
    private BackButton _backButton;
    
    private float _startTime;

    private void Awake()
    {
        _popUpTexts = GetComponent<PopUpTexts>();
    }

    private void Start()
    {
        _palette = FindObjectOfType<Palette>();
        _limitedChances = FindAnyObjectByType<LimitedChances>();
        _resetButton = FindAnyObjectByType<ResetButton>();
        _board = FindAnyObjectByType<Board>();
        _targetColorText = FindAnyObjectByType<TargetColorText>();
        _backButton = FindAnyObjectByType<BackButton>();
        
        RegisterSelectedColor();
        RegisterCell();
        RegisterResetButton();
        RegisterBackButton();
        
        GameStart();
        
        _startTime = Time.time;
    }

    private void OnDestroy()
    {
        ResetSelectedColor();
        ResetCell();
        ResetResetButton();
        ResetBackButton();       
    }

    private void GameStart()
    {
        StageData stageData = StageSaveLoader.Stages[GameManager.Instance.StageNum];
        _board.SetStageBoard(stageData.board);
        _targetColorText.SetTargetColor(stageData.targetColor);
        _limitedChances.SetChances(stageData.chances);
        
        _palette.UpdatePaletteVisibility();
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
    
    public void FloodFill(int startR, int startC, CellColor newColor)
    {
        // 1. 현재 플러드 영역의 색상 (Flood-It에서 변경될 색상)
        CellColor oldColor = _board.Cells[startR, startC].Color; 

        // 2. 이미 같은 색이면 종료 (불필요한 이동 방지)
        if (oldColor == newColor)
            return;

        // BFS (Queue)를 사용하여 구현하는 것이 재귀(DFS) 스택 오버플로우 방지에 더 안전합니다.
        var queue = new Queue<(int r, int c)>();
        queue.Enqueue((startR, startC));

        // 시작 셀의 색상 변경 (변경은 큐에 넣기 전에 한 번만 수행)
        _board.Cells[startR, startC].ChangeColor(newColor); 

        //방향
        int[] dr = { 1, -1, 0, 0 }; 
        int[] dc = { 0, 0, 1, -1 };

        while (queue.Count > 0)
        {
            var (r, c) = queue.Dequeue();

            for (int i = 0; i < dr.Length; i++)
            {
                int nr = r + dr[i];
                int nc = c + dc[i];

                // 3. 경계 및 색상 조건 확인
                if (nr >= 0 && nr < Board.Rows && nc >= 0 && nc < Board.Cols && 
                    _board.Cells[nr, nc].Color == oldColor) // 인접 셀이 '이전 플러드 영역의 색상'과 같으면
                {
                    _board.Cells[nr, nc].ChangeColor(newColor); // 새 색상으로 칠하고
                    queue.Enqueue((nr, nc));   // 큐에 추가
                }
            }
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
                _board.Cells[i,j].OnCellClicked += () => FloodFill(x,y,_selectedColor);;
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

    private void CheckClear()
    {
        bool clear = true;
        
        for(int i=0;i<Board.Rows;i++)
        {
            for(int j=0;j<Board.Cols;j++)
            {
                if(_board.Cells[i,j].Color != _targetColorText.Color)
                    clear = false;
            }
        }

        if (clear)
        {
            ClearPopUp();
            EventLogStageClear();
        }
        else if(_limitedChances.Chances == 0)
            FailPopUp();
    }

    private async void ClearPopUp()
    {
        GameObject go = await UIPrefabManager.Instance.ShowUI(UIPrefabs.Alert);
        
        if (go.TryGetComponent(out AlertPopUp alertPopUp))
        {
            alertPopUp.gameObject.SetActive(true);
            alertPopUp.SetDescription(_popUpTexts.CompleteText);
            alertPopUp.OkButton.onClick.AddListener(() => SceneMng.ChangeScene(SceneName.LobbyScene));
        }
    }

    private void EventLogStageClear()
    {
        //Firebase 분석 로그
        int stageId = GameManager.Instance.StageNum + 1;
        float clearTime = Time.time - _startTime;
        int usedChance =  _limitedChances.FirstChances - _limitedChances.Chances;
        int retryCount = _limitedChances.ResetCount;
        
        FirebaseManager.Instance.Analytic.StageClearLog(stageId, clearTime, usedChance, retryCount);
    }
    
    private async void FailPopUp()
    {
        GameObject go = await UIPrefabManager.Instance.ShowUI(UIPrefabs.Alert);

        if (go.TryGetComponent(out AlertPopUp alertPopUp))
        {
            alertPopUp.gameObject.SetActive(true);
            alertPopUp.SetDescription(_popUpTexts.FailText);
            alertPopUp.OkButton.onClick.AddListener(_board.ResetBoard);
            alertPopUp.OkButton.onClick.AddListener(_limitedChances.ResetChances);
        }
    }
    
    private void RegisterBackButton()
    {
        _backButton.Button.onClick.AddListener(()=>SceneMng.ChangeScene(SceneName.LobbyScene));
    }
    
    private void ResetBackButton()
    {
        _backButton.Button.onClick.RemoveAllListeners();
    }
}
