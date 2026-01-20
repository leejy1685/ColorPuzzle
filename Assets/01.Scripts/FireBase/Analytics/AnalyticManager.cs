using Firebase.Analytics;
using UnityEngine;

public class AnalyticManager : MonoBehaviour
{
    public void InitializeUserId()
    {
        // 1. 기기의 고유 식별자를 가져옵니다. 
        string deviceId = SystemInfo.deviceUniqueIdentifier;

        // 2. Firebase에 이 기기를 이 ID로 기억하라고 명령합니다.
        FirebaseAnalytics.SetUserId(deviceId);
    }
    
    /// <summary>
    /// 클리어 정보를 Firebase로 수집
    /// </summary>
    /// <param name="stageId"></param>
    /// <param name="clearTime"></param>
    /// <param name="usedChance"></param>
    /// <param name="retryCount"></param>
    public void StageClearLog(int stageId, float clearTime, int usedChance, int retryCount)
    {
        ClearInfo info = new ClearInfo(stageId, clearTime, usedChance, retryCount);
        
        FirebaseAnalytics.LogEvent("clearInfo",info.Parameters);
    }
    
    
}
