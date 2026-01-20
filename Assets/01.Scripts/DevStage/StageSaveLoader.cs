using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public static class StageSaveLoader
{
    private static readonly string EDITOR_PATH = Path.Combine(Application.streamingAssetsPath, "Stages.json");
    
    public static List<StageData> Stages { get; private set; }

    public static void SaveStage(StageData stage)
    {
        // 저장을 시도할 때 Stages 리스트가 초기화되지 않았다면 새로 생성합니다.
        if (Stages == null)
        {
            Stages = new List<StageData>();
        }
        Stages.Add(stage);
        
        Directory.CreateDirectory(Path.GetDirectoryName(EDITOR_PATH));
        string json = JsonConvert.SerializeObject(Stages, Formatting.Indented);
        File.WriteAllText(EDITOR_PATH, json);
    }
    
    /// <summary>
    /// 스테이지 데이터를 UniTask를 사용해 비동기적으로 로드합니다.
    /// 로드된 데이터를 직접 반환하거나 실패 시 예외를 던집니다.
    /// </summary>
    public static async UniTask<List<StageData>> LoadStagesAsync()
    {
        try
        {
            string json = "";

#if UNITY_EDITOR
            if (File.Exists(EDITOR_PATH))
            {
                json = await File.ReadAllTextAsync(EDITOR_PATH);
            }
#else
            UnityWebRequest www = UnityWebRequest.Get(EDITOR_PATH);
            
            await www.SendWebRequest();
            
            json = www.downloadHandler.text;
#endif

            if (!string.IsNullOrEmpty(json))
            {
                Stages = JsonConvert.DeserializeObject<List<StageData>>(json);
            }
            
            if (Stages == null)
            {
                Stages = new List<StageData>();
            }
            
            return Stages;
        }
        catch (Exception e)
        {
            Debug.LogError($"[StageSaveLoader] 스테이지 로드 실패: {e.Message}");
            Stages = new List<StageData>(); 
            throw; 
        }
    }

}
