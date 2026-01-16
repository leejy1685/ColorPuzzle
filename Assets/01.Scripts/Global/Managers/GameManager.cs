using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }
    
    private int _stageNum;

    public int StageNum
    {
        get {return _stageNum;}
        set {_stageNum = value;}
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        //화면 꺼짐 방지
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        FileLoad();
    }

    private async void FileLoad()
    {
        Debug.Log("스테이지 데이터 로딩을 시작합니다...");
        try
        {
            var loadedStages = await StageSaveLoader.LoadStagesAsync();
            
            Debug.Log($"스테이지 로딩 완료! 로드된 스테이지 개수: {loadedStages.Count}");
        }
        catch (Exception e)
        {
            Debug.LogError($"스테이지 로딩에 실패했습니다: {e.Message}");
        }

    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }
}
