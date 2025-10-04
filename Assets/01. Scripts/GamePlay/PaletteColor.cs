using UnityEngine;
using UnityEngine.EventSystems;

public class PaletteColor : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CellColor color;
    
    public CellColor Color => color;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
            Debug.Log(color);//현재 선택된 색
        
        if(eventData.button == PointerEventData.InputButton.Right)
            Debug.Log("right Button");
        
        if(eventData.button == PointerEventData.InputButton.Middle)
            Debug.Log("middle Button");

    }
}
