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
    private static readonly string EDITOR_PATH = Path.Combine(Application.dataPath, "StreamingAssets", "Stages.json");
    
    public static List<StageData> Stages { get; private set; }

    public static void SaveStage(StageData stage)
    {
        // 저장을 시도할 때 Stages 리스트가 초기화되지 않았다면 새로 생성합니다.
        if (Stages == null)
        {
            Stages = new List<StageData>();
        }
        Stages.Add(stage);
        
#if UNITY_EDITOR
        Directory.CreateDirectory(Path.GetDirectoryName(EDITOR_PATH));
        string json = JsonConvert.SerializeObject(Stages, Formatting.Indented);
        File.WriteAllText(EDITOR_PATH, json);
        Debug.Log($"스테이지 저장됨: {EDITOR_PATH}");
#endif
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
                // 파일을 비동기적으로 읽습니다.
                json = await File.ReadAllTextAsync(EDITOR_PATH);
            }
#else
            string streamingPath = Path.Combine(Application.streamingAssetsPath, "Stages.json");
            UnityWebRequest www = UnityWebRequest.Get(streamingPath);
            
            // SendWebRequest()를 await로 기다립니다. 에러가 발생하면 예외를 던집니다.
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
                Debug.Log("[StageSaveLoader] 새 스테이지 리스트 생성됨 (파일이 없거나 비어 있음)");
            }

            Debug.Log($"[StageSaveLoader] 스테이지 로드 성공: {Stages.Count}개");
            return Stages;
        }
        catch (Exception e)
        {
            Debug.LogError($"[StageSaveLoader] 스테이지 로드 실패: {e.Message}");
            Stages = new List<StageData>(); // 실패 시 빈 리스트로 초기화
            throw; // 예외를 호출자에게 다시 던져서 로딩 실패를 알립니다.
        }
    }

}
