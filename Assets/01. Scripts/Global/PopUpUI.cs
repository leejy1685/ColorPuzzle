using System;
using TMPro;
using UnityEngine;

public class PopUpUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    
    [SerializeField] private string titleText;
    [SerializeField][TextArea(2,2)] private string clearText;
    [SerializeField][TextArea(2,2)] private string failText;

    public Action OnRetry;

    public void ClearPopUp()
    {
        description.text = clearText;
    }
    
    public void FailPopUp()
    {
        description.text = failText;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            OnRetry.Invoke();
    }
}
