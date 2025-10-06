using System;
using UnityEngine;

public class Board : MonoBehaviour
{
    public const int Rows = 10;
    public const int Cols = 8;
    
    private Cell[,] _cells;
    
    public Cell[,] Cells => _cells;

    private void Awake()
    {
        Convert1DTo2D();
    }

    private void Convert1DTo2D()
    {
        Cell[] cell = GetComponentsInChildren<Cell>();
        _cells = new Cell[Rows, Cols];

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                int k = i * Cols + j;
                _cells[i, j] = cell[k];
            }
        }
    }
}
