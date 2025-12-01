using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageContainer : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject stageButton;
    
    private void Awake()
    {
        for (int i = 0; i < StageSaveLoader.Stages.Count; i++)
        {
            GameObject go = ObjectPool.Get(PoolIndex.StageButton, stageButton);
            go.transform.SetParent(content.transform);

            if (go.TryGetComponent(out StageButton stageBtn))
            {
                stageBtn.StageData = StageSaveLoader.Stages[i];
                stageBtn.SetStageName($"Stage {i + 1}");
            }
        }
    }

}
