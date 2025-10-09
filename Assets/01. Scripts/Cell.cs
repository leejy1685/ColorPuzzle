using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CellColor color;
    
    private Image _image;
    
    public Action OnCellClicked;
    
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
            OnCellClicked.Invoke();
    }
}
