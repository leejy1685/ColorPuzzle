using UnityEngine;

public class UICanvasMarker : MonoBehaviour
{
    private void OnEnable()
    {
        if (UIPrefabManager.Instance != null)
        {
            UIPrefabManager.Instance.RegisterMainCanvas(transform);
        }
    }

    private void OnDisable()
    {
        if (UIPrefabManager.Instance != null)
        {
            UIPrefabManager.Instance.UnregisterMainCanvas();
        }
    }

}
