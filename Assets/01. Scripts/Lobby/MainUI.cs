using System;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] private Button gamePlay;
    [SerializeField] private Button stageDev;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject stageContainerPrefab;

    private Canvas _canvas;
    private StageContainer stageContainer;

    private void Awake()
    {
        gamePlay.onClick.AddListener(GamePlay);
        stageDev.onClick.AddListener(StageDev);
        quitButton.onClick.AddListener(Quit);
        
    }   

    private void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
        
        GameObject go = ObjectPool.Get(PoolIndex.StageContainer, stageContainerPrefab);
        go.transform.SetParent(_canvas.transform);
        go.transform.localPosition = Vector3.zero;
        
        go.TryGetComponent(out stageContainer);
    }

    private void GamePlay()
    {
        stageContainer.gameObject.SetActive(true);
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
