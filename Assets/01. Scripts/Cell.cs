using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private CellColor color;
    
    private Image _image;
    
    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.color = ColorConverter.ColorCodeToColor(color);
    }

    public void ChangeColor(CellColor newColor)
    {
        _image.color = ColorConverter.ColorCodeToColor(newColor);
    }
    
}
