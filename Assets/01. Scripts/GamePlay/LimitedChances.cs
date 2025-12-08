using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LimitedChances : MonoBehaviour
{
    private int _firstChances;
    private TextMeshProUGUI _text;
    private int _chances;

    public int Chances => _chances;
    
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        
        _chances = _firstChances;
        _text.text = _chances.ToString();
    }

    public void SetChances(int chances)
    {
        _chances = chances;
        _text.text = _chances.ToString();
        _firstChances = chances;
    }
    
    public void UsingChances()
    {
        _chances--;
        _text.text = _chances.ToString();
    }
    
    public void ResetChances()
    {
        _chances = _firstChances;
        _text.text = _chances.ToString();
    }
    
    
    
    
    
}
