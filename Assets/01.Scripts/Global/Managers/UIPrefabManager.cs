using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum UIPrefabs
{
    Alert,
    Confirm,
    StageButton,
    StageContainer,
}

/// <summary>
/// UI 프리팹을 로드하고, 인스턴스화된 객체와 Addressables 핸들을 매핑하여 관리합니다.
/// </summary>
public class UIPrefabManager : MonoBehaviour
{
    private static UIPrefabManager _instance;

    public static UIPrefabManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIPrefabManager>();

                if (_instance != null)
                {
                    DontDestroyOnLoad(_instance.gameObject);
                }
            }

            return _instance;
        }
    }
    
    private Transform _currentMainCanvas;
    
    private Dictionary<GameObject, AsyncOperationHandle<GameObject>> _activePrefabHandles = 
        new Dictionary<GameObject, AsyncOperationHandle<GameObject>>();

    private void Awake()
    {
        // 인스턴스가 아직 설정되지 않았거나, 현재 인스턴스가 나라면
        if (_instance == null || _instance == this)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else // 이미 다른 인스턴스가 존재하면
        {
            Destroy(gameObject); // 나는 파괴
        }

    }

    public void RegisterMainCanvas(Transform canvas)
    {
        _currentMainCanvas = canvas;
    }

    public void UnregisterMainCanvas()
    {
        _currentMainCanvas = null;
    }


    public async UniTask<GameObject> ShowUI(UIPrefabs address)
    {
        AsyncOperationHandle<GameObject> loadHandle = default;
        try
        {
            if (_currentMainCanvas == null)
                throw new InvalidOperationException("UI 캔버스 없음.");
            
            loadHandle = ResourceLoader.LoadAssetAsync<GameObject>(address.ToString());
            GameObject prefabAsset = await loadHandle;
            
            GameObject instance = ObjectPool.Get(prefabAsset, _currentMainCanvas);
            _activePrefabHandles.Add(instance, loadHandle);
            
            return instance;
        }
        catch (Exception e)
        {
            Debug.LogError($"UI '{address}' 로드 실패: {e.Message}");
            if (loadHandle.IsValid())
            {
                ResourceLoader.Release(loadHandle);
            }
            return null;
        }
    }
    
    public void CloseUI(GameObject instance)
    {
        if (_activePrefabHandles.TryGetValue(instance, out AsyncOperationHandle<GameObject> handle))
        {
            ObjectPool.Release(instance);
            ResourceLoader.Release(handle); 
            _activePrefabHandles.Remove(instance);
        }
        else
        {
            Destroy(instance); 
        }
    }
}