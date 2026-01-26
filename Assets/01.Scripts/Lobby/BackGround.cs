using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class BackGround : MonoBehaviour
{
    private const int WIDTH = 4;
    private const int HEIGHT = 3;
    
    [SerializeField] private float changeTime;
    
    private Cell[] _cells;
    private GridLayoutGroup _gridLayoutGroup;

    private void Awake()
    {
        _cells = GetComponentsInChildren<Cell>();
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        StartCoroutine(ChangeBG_Coroutine());
        ApplyScreenRatio();
    }

    private IEnumerator ChangeBG_Coroutine()
    {
        while (true)
        {
            foreach (Cell cell in _cells)
            {
                cell.ChangeColor(ColorConverter.RandomColor());
            }
            yield return new WaitForSeconds(changeTime);
        }
    }

    private void ApplyScreenRatio()
    {
        float xSpacing = _gridLayoutGroup.spacing.x;
        float ySpacing = _gridLayoutGroup.spacing.y;

        float cellWidth = (float)Screen.width / WIDTH - xSpacing;
        float cellHeight = (float)Screen.height / HEIGHT - ySpacing;
        
        _gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
