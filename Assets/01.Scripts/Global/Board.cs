using UnityEngine;

public class Board : MonoBehaviour
{
    public const int Rows = 8;
    public const int Cols = 10;
    
    private Cell[,] _cells;
    private CellColor[,] _firstCells;
    
    public Cell[,] Cells => _cells;
    public CellColor[,] FirstCells => _firstCells;
    public CellColor[,] CurrentCells
    {
        get
        {
            CellColor[,] cells = new CellColor[Rows, Cols];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    cells[i, j] = _cells[i, j].Color;
                }
            }
            return cells;
        }
    }

    
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
    
    public void SetStageBoard(CellColor[,] cells)
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                _cells[i,j].ChangeColor(cells[i,j]);
            }
        }

        _firstCells = cells;
    }

    public bool HasTargetColor(CellColor color)
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                if (_cells[i,j].Color == color) return true;
            }
        }
        return false;
    }
    
    
}
