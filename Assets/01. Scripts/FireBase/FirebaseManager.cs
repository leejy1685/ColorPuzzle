using UnityEngine;
using Firebase;
using Firebase.Analytics;

public class FirebaseManager : MonoBehaviour
{
    void Start()
    {
        // 1. Firebase 종속성 및 상태 확인
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            
            if (dependencyStatus == DependencyStatus.Available) {
                // 2. 성공 시 Firebase 초기화
                FirebaseApp app = FirebaseApp.DefaultInstance;
                Debug.Log("Firebase가 준비되었습니다!");

                // 3. 테스트 이벤트 보내기 (서버로 'app_open' 신호를 보냄)
                FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen);
                
                // 나만의 커스텀 이벤트 보내기
                FirebaseAnalytics.LogEvent("test_event_success");
            } else {
                Debug.LogError($"Firebase를 사용할 수 없습니다: {dependencyStatus}");
            }
        });
    }
}