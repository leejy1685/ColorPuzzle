using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTargetColorButton : MonoBehaviour
{
    private Button _button;
    
    public Button Button => _button;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    
    
}
