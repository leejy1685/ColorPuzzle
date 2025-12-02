using System;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] private Button gamePlay;
    [SerializeField] private Button stageDev;
    [SerializeField] private Button quitButton;

    private Canvas _canvas;
    private StageContainer _stageContainer;

    private void Awake()
    {
        gamePlay.onClick.AddListener(GamePlay);
        stageDev.onClick.AddListener(StageDev);
        quitButton.onClick.AddListener(Quit);
        
    }   

    private async void Start()
    {
        GameObject go = await UIPrefabManager.Instance.ShowUI(UIPrefabs.StageContainer);
        go.TryGetComponent(out _stageContainer);
    }

    private void GamePlay()
    {
        _stageContainer.gameObject.SetActive(true);
    }

    private void StageDev()
    {
        SceneMng.ChangeScene(SceneName.DevStageScene);
    }

    private void Quit()
    {
        Application.Quit();
    }
    
    
    
}
