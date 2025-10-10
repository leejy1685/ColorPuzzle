using UnityEngine;

public class Board : MonoBehaviour
{
    public const int Rows = 8;
    public const int Cols = 10;
    
    private Cell[,] _cells;
    private CellColor[,] _firstCells;
    
    public Cell[,] Cells => _cells;

    private void Awake()
    {
        Convert1DTo2D();
    }

    private void Convert1DTo2D()
    {
        Cell[] cell = GetComponentsInChildren<Cell>();
        _cells = new Cell[Rows, Cols];
        _firstCells = new CellColor[Rows, Cols];

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                int k = i * Cols + j;
                _cells[i, j] = cell[k];
                _firstCells[i,j] = cell[k].Color;
            }
        }
    }

    public void ResetBoard()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                _cells[i,j].ChangeColor(_firstCells[i,j]);
            }
        }
    }
    
    
}
