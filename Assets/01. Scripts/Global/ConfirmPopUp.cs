using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    
    public Button YesButton => yesButton;
    public Button NoButton => noButton;
    
    public void SetTitle(string text)
    {
        title.text = text;
    }

    public void SetDescription(string text)
    {
        description.text = text;
    }

    private void OnEnable()
    {
        YesButton.onClick.AddListener(()=>ObjectPool.Release(PoolIndex.Confirm,gameObject));
        NoButton.onClick.AddListener(()=>ObjectPool.Release(PoolIndex.Confirm,gameObject));
    }

    public void OnDisable()
    {
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
    }
}
