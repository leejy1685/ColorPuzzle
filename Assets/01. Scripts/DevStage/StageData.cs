using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData
{
    public int stageId;
    public int chances;
    public CellColor targetColor;
    public CellColor[,] board;
    
    public StageData(int stageId, int chances, CellColor targetColor, CellColor[,] board)
    {
        this.stageId = stageId;
        this.chances = chances;
        this.targetColor = targetColor;
        this.board = board;
    }

    public void Print()
    {
        Debug.Log(stageId);
        Debug.Log(chances);
        Debug.Log(targetColor);
        
        foreach (CellColor cellColor in board)
        {
            Debug.Log(cellColor);
        }
    }
}
