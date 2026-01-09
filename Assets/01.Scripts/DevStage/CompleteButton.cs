using UnityEngine;
using UnityEngine.UI;

public class CompleteButton : MonoBehaviour
{
    private Button _button;
    
    public Button Button => _button;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
    }
}
