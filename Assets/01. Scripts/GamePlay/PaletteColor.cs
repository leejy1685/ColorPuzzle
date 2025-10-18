using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PaletteColor : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CellColor color;
    [SerializeField] private GameObject outLine;
    
    public Action OnColorSelected;
    
    public CellColor Color => color;

    private void Awake()
    {
        outLine.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnColorSelected.Invoke();
        }
    }

    public void SetOutLine(bool isOn)
    {
        outLine.SetActive(isOn);
    }
}
