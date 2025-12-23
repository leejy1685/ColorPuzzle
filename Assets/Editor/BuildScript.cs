using System;
using System.Collections.Generic;
using UnityEditor;

namespace Editor
{
    public class BuildScript
    {
        static string ScenePath = "Assets/00. Scenes/";

        // Jenkins가 호출할 Windows 64비트 빌드 함수
        public static void BuildWindows()
        {
            BuildPlayerOptions options = new BuildPlayerOptions();


            // 1. 빌드한 씬 목록 지정
            List<string> scenes = new List<string>();
            foreach (SceneName sceneName in Enum.GetValues(typeof(SceneName)))
            {
                string scenePath = $"{ScenePath}{sceneName}.unity";
                scenes.Add(scenePath);
            }

            options.scenes = scenes.ToArray();

            // 2. 결과물 경로 및 이름 지정 (확장자는 .exe를 붙이지 않습니다)
            options.locationPathName = "Builds/MyGame/MyGame.exe";

            // 3. 빌드 타겟을 Windows 64비트로 지정
            options.target = BuildTarget.StandaloneWindows64;

            options.targetGroup = BuildTargetGroup.Standalone;

            options.options = BuildOptions.None; // 기본 옵션

            BuildPipeline.BuildPlayer(options);
        }
    }
}












