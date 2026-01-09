using System;
using UnityEngine.SceneManagement;

public enum SceneName
{
    LobbyScene,
    MainScene,
    DevStageScene
}

public static class SceneMng
{
    public static void ChangeScene(SceneName sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
}
