using UnityEngine;

public class PopUpTexts : MonoBehaviour
{
    [SerializeField] [TextArea(2, 2)] private string targetColorText;
    [SerializeField] [TextArea(2, 2)] private string completeText;
    [SerializeField] [TextArea(2, 2)] private string failText;
    
    public string TargetColorText => targetColorText;
    public string CompleteText => completeText;
    public string FailText => failText;
}
