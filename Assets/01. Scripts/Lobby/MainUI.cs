using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] private Button gamePlay;
    [SerializeField] private Button stageDev;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject stages;

    private void Awake()
    {
        gamePlay.onClick.AddListener(GamePlay);
        quitButton.onClick.AddListener(Quit);
    }

    private void GamePlay()
    {
        stages.SetActive(true);
    }

    private void Quit()
    {
        Application.Quit();
    }
    
    
    
}
