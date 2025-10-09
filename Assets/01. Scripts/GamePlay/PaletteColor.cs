using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PaletteColor : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CellColor color;
    
    public Action OnColorSelected;
    
    public CellColor Color => color;
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
            OnColorSelected.Invoke();
    }
}
