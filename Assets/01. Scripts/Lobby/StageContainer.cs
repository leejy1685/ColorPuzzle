using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageContainer : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject stageButton;
    
    private BackButton _backButton;
    
    private void Awake()
    {
        _backButton = GetComponentInChildren<BackButton>();
    }

    private void Start()
    {
        SetStageButton();
        SetBackButton();
    }

    private async void SetStageButton()
    {
        for (int i = 0; i < StageSaveLoader.Stages.Count; i++)
        {
            GameObject go = await UIPrefabManager.Instance.ShowUI(UIPrefabs.StageButton);
            go.transform.SetParent(content.transform);

            if (go.TryGetComponent(out StageButton stageBtn))
            {
                stageBtn.StageNum = i;
                stageBtn.SetStageName($"Stage {i + 1}");
            }
        }
    }
    
    private void SetBackButton()
    {
        _backButton.Button.onClick.AddListener(()=>UIPrefabManager.Instance.CloseUI(gameObject));
    }

}
