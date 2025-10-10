using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResetButton : MonoBehaviour, IPointerClickHandler
{
    public Action OnReset;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnReset.Invoke();
    }
}
