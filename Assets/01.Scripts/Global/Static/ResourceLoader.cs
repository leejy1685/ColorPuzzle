using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

//<summary>
//어드레서블 이용한 리소스 로더
//</summary>
public static class ResourceLoader
{
    // 정적 함수로 Addressables 로드 요청을 감쌉니다.
    public static AsyncOperationHandle<T> LoadAssetAsync<T>(string address) where T : Object
    {
        return Addressables.LoadAssetAsync<T>(address);
    }
    
    // 정적 함수로 Addressables 해제 요청을 감쌉니다.
    public static void Release<T>(AsyncOperationHandle<T> handle) where T : Object
    {
        if (handle.IsValid())
        {
            Addressables.Release(handle);
        }
    }
}