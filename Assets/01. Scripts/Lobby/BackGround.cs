using System.Collections;
using UnityEngine;


public class BackGround : MonoBehaviour
{
    [SerializeField] private float changeTime;
    
    private Cell[] _cells;
    

    private void Start()
    {
        _cells = GetComponentsInChildren<Cell>();
        
        StartCoroutine(ChangeBG_Coroutine());
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
}
