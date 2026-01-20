using UnityEngine;
using Firebase;


public class FirebaseManager : MonoBehaviour
{
    private static FirebaseManager _instance;
    public static FirebaseManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<FirebaseManager>();
            return _instance;
        }
    }
    
    
    private AnalyticManager _analytic;
    
    public AnalyticManager Analytic => _analytic;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        _analytic = GetComponent<AnalyticManager>();
    }

    void Start()
    {
        // 1. Firebase 종속성 및 상태 확인
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => 
        {
            var dependencyStatus = task.Result;
            
            if (dependencyStatus == DependencyStatus.Available) 
            {
                // 2. 성공 시 Firebase 초기화
                FirebaseApp app = FirebaseApp.DefaultInstance;

                //3. User 등록
                _analytic.InitializeUserId();
                
            } 
            else 
            {
                Debug.LogError($"Firebase를 사용할 수 없습니다: {dependencyStatus}");
            }
        });
    }
}