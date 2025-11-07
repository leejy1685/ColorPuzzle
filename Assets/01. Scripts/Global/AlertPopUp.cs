using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertPopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Button okButton;
    
    public Button OkButton => okButton;

    public void SetTitle(string text)
    {
        title.text = text;
    }

    public void SetDescription(string text)
    {
        description.text = text;
    }


}
