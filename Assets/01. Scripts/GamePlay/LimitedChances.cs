using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LimitedChances : MonoBehaviour
{
    [SerializeField] private int firstChances;
    
    private TextMeshProUGUI _text;
    private int _chances;
    
    public int Chances => _chances;
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        
        _chances = firstChances;
        _text.text = _chances.ToString();
    }
    
    public void UsingChances()
    {
        _chances--;
        _text.text = _chances.ToString();
    }
    
    public void ResetChances()
    {
        _chances = firstChances;
        _text.text = _chances.ToString();
    }
    
    
    
    
    
}
