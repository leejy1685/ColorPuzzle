using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    private int _stageNum;
    private Button _button;
    private TextMeshProUGUI _text;
    
    public int StageNum
    {
        get {return _stageNum;}
        set {_stageNum = value;}
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

    private void OnDestroy()
    {
        UIPrefabManager.Instance.CloseUI(gameObject);
    }

    private void LoadStage()
    {
        GameManager.Instance.StageNum = _stageNum;
        SceneMng.ChangeScene(SceneName.MainScene);
    }

    public void SetStageName(string name)
    {
        _text.text = name;
    }
    
    
    
    
}
