using System.Collections.Generic;
using UnityEngine;

public enum PoolIndex
{
    Alert,
    Confirm,
}

public static class ObjectPool
{
    private static Dictionary<PoolIndex,Queue<GameObject>> _pool = new Dictionary<PoolIndex, Queue<GameObject>>();

    public static GameObject Get(PoolIndex type,GameObject obj)
    {
        //키가 존재하고 Queue에 오브젝트가 있을 때
        if (_pool.TryGetValue(type, out Queue<GameObject> queue) && queue.Count > 0)
        {
            GameObject go = queue.Dequeue();
            go.SetActive(true);
            return go;
        }
        
        //생성
        return Object.Instantiate(obj);
    }

    public static void Release(PoolIndex type, GameObject obj)
    {
        if (obj == null) return;
        
        //오브젝트 비활성화
        obj.SetActive(false);
        
        //키가 없으면 생성
        if (!_pool.ContainsKey(type))
        {
            _pool.Add(type, new Queue<GameObject>());
        }
        
        //Queue에 넣기
        _pool[type].Enqueue(obj);
        
    }
}
