using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour,IPointerEnterHandler,IPointerDownHandler
{
    [SerializeField] private CellColor color;
    
    private Image _image;
    
    public Action OnCellClicked;
    public Action OnCellHovered;
    
    public CellColor Color => color;
    
    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.color = ColorConverter.ColorCodeToColor(color);
    }

    public void ChangeColor(CellColor newColor)
    {
        color = newColor;
        _image.color = ColorConverter.ColorCodeToColor(newColor);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (OnCellHovered == null)
                return;
            OnCellHovered.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (OnCellClicked == null)
                return;
            OnCellClicked.Invoke();
        }
    }
}
