using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool
{
    private static Dictionary<GameObject,Queue<GameObject>> _pool = new Dictionary<GameObject, Queue<GameObject>>();

    public static GameObject Get(GameObject originalPrefab,Transform parent = null)
    {
        //키가 존재하고 Queue에 오브젝트가 있을 때
        if (_pool.TryGetValue(originalPrefab, out Queue<GameObject> queue) && queue.Count > 0)
        {
            GameObject go = queue.Dequeue();
            if (go != null)
            {
                go.SetActive(true);
                return go;
            }
        }
        
        //생성
        GameObject newInstance = Object.Instantiate(originalPrefab, parent);
        
        //구분을 위한 컴포넌트 추가
        PoolItem item = newInstance.AddComponent<PoolItem>();
        item.originalPrefab = originalPrefab;

        return newInstance;
    }

    public static void Release(GameObject instance)
    {
        if (instance == null) return;
        
        //오브젝트 비활성화
        instance.SetActive(false);
        
        // 1. 🛠️ 인스턴스에서 원본 프리팹 키를 추출합니다.
        PoolItem item = instance.GetComponent<PoolItem>();
        if (item == null || item.originalPrefab == null)
        {
            Object.Destroy(instance); // 풀링 객체가 아니므로 파괴
            return;
        }

        GameObject originalPrefabKey = item.originalPrefab;

        // 2. 원본 프리팹 키를 사용하여 Queue에 접근합니다.
        if (!_pool.ContainsKey(originalPrefabKey))
        {
            _pool.Add(originalPrefabKey, new Queue<GameObject>());
        }
        
        // 3. 인스턴스를 원본 키의 Queue에 넣기
        _pool[originalPrefabKey].Enqueue(instance);
        
    }
}
