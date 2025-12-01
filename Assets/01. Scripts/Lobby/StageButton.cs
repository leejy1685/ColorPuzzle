using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    private StageData _stageData;
    private Button _button;
    private TextMeshProUGUI _text;


    public StageData StageData
    {
        get {return _stageData;}
        set {_stageData = value;}
    }
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(LoadStage);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void LoadStage()
    {
        GameManager.Instance.StageData = _stageData;
        SceneMng.ChangeScene(SceneName.MainScene);
    }

    public void SetStageName(string name)
    {
        _text.text = name;
    }
    
    
}
