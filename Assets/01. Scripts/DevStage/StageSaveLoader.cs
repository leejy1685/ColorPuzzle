using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class StageSaveLoader
{
    //데이터 저장 메서드
    public static void SaveStage(StageData stage)
    {
        FileStream stream = new FileStream(Application.dataPath+$"\\Stage{stage.stageId}.json",FileMode.Create);
        
        string json = JsonConvert.SerializeObject(stage,Formatting.Indented);
        
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        
        stream.Write(bytes,0,bytes.Length);
        
        stream.Close();
    }

    //데이터 불러오기 메서드
    public static StageData LoadStage(int stageId)
    {
        FileStream stream = new FileStream(Application.dataPath+$"\\Stage{stageId}.json",FileMode.Open);
        
        byte[] data = new byte[stream.Length];
        
        stream.Read(data,0,data.Length);
        
        stream.Close();
        
        string json = System.Text.Encoding.UTF8.GetString(data);
        
        StageData stage = JsonConvert.DeserializeObject<StageData>(json);
        
        return stage;
    }
}
