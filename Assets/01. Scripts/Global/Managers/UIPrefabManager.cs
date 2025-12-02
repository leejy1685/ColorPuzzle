using System.Collections.Generic;
using System.Threading.Tasks;
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
    public static UIPrefabManager Instance { get; private set; }
    
    private Dictionary<GameObject, AsyncOperationHandle<GameObject>> activePrefabHandles = 
        new Dictionary<GameObject, AsyncOperationHandle<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public async Task<GameObject> ShowUI(UIPrefabs address)
    {
        AsyncOperationHandle<GameObject> loadHandle = ResourceLoader.LoadAssetAsync<GameObject>(address.ToString());
        
        await loadHandle.Task;

        if (loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject prefabAsset = loadHandle.Result;
            Transform uiParent = FindObjectOfType<Canvas>().transform;
            GameObject instance = ObjectPool.Get(prefabAsset, uiParent);
            
            activePrefabHandles.Add(instance, loadHandle);
            
            return instance;
        }
        else
        {
            ResourceLoader.Release(loadHandle);
            
            return null;
        }
    }
    
    public void CloseUI(GameObject instance)
    {
        if (activePrefabHandles.TryGetValue(instance, out AsyncOperationHandle<GameObject> handle))
        {
            ObjectPool.Release(instance);
            
            ResourceLoader.Release(handle); 
            
            activePrefabHandles.Remove(instance);
        }
        else
        {
            Destroy(instance); 
        }
    }
}