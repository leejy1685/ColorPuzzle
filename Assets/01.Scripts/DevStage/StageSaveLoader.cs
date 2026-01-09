using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public static class StageSaveLoader
{
    private static readonly string FILE_PATH = $"{Application.dataPath}\\Stages.json";
    
    private static List< StageData> _stages;
    public static List< StageData> Stages
    {
        get
        {
            if (_stages == null) { LoadStage(); }
            return _stages;
        }
    }

    //데이터 저장 메서드
    public static void SaveStage(StageData stage)
    {
        //스테이지 추가
        Stages.Add(stage);
        
        //데이터 저장
        FileStream stream = new FileStream(FILE_PATH,FileMode.OpenOrCreate);
        string json = JsonConvert.SerializeObject(Stages,Formatting.Indented);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        stream.Write(bytes,0,bytes.Length);
        stream.Close();
    }

    //데이터 불러오기 메서드
    public static void LoadStage()
    {
        //데이터 불러오기
        FileStream stream = new FileStream(FILE_PATH,FileMode.OpenOrCreate);
        byte[] data = new byte[stream.Length];
        stream.Read(data,0,data.Length);
        stream.Close();
        
        //역직렬화
        string json = System.Text.Encoding.UTF8.GetString(data);
        _stages = JsonConvert.DeserializeObject<List<StageData>>(json);

        //만약 저장된 데이터가 없을 때 리스트 생성
        if (_stages == null) { _stages = new List<StageData>(); }
    }
}
