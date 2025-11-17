using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class StageSaveLoader
{
    private static Dictionary<int, StageData> _stages;
    private static int stageCount;
    public static Dictionary<int, StageData> Stages
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
        Stages.Add(++stageCount,stage);
        
        //데이터 저장
        FileStream stream = new FileStream(Application.dataPath + "\\Stages.json",FileMode.OpenOrCreate);
        string json = JsonConvert.SerializeObject(Stages,Formatting.Indented);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        stream.Write(bytes,0,bytes.Length);
        stream.Close();
    }

    //데이터 불러오기 메서드
    public static void LoadStage()
    {
        //데이터 불러오기
        FileStream stream = new FileStream(Application.dataPath+$"\\Stages.json",FileMode.OpenOrCreate);
        byte[] data = new byte[stream.Length];
        stream.Read(data,0,data.Length);
        stream.Close();
        
        //역직렬화
        string json = System.Text.Encoding.UTF8.GetString(data);
        _stages = JsonConvert.DeserializeObject<Dictionary<int,StageData>>(json);
        
        //스테이지 숫자 저장
        if (_stages == null) { _stages = new Dictionary<int, StageData>(); }
        stageCount = _stages.Count;
    }
}
