using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData
{
    public int chances;
    public CellColor targetColor;
    public CellColor[,] board;
    
    //생성자
    public StageData(int chances, CellColor targetColor, CellColor[,] board)
    {
        this.chances = chances;
        this.targetColor = targetColor;
        this.board = board;
    }

    //Test Code
    public void Print()
    {
        Debug.Log(chances);
        Debug.Log(targetColor);
        
        foreach (CellColor cellColor in board)
        {
            Debug.Log(cellColor);
        }
    }
}
