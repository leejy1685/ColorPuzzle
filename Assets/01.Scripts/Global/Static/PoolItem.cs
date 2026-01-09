using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 인스턴스가 원본 프리팹을 기억하도록 하는 컴포넌트
public class PoolItem : MonoBehaviour
{
    // 이 인스턴스를 생성한 원본 프리팹(키)을 저장
    public GameObject originalPrefab;
}
