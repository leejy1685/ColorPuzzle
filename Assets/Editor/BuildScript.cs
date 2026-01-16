using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

public enum BuildPlatform
{
    Windows,
    Android
}

namespace Editor
{
    public class BuildScript
    {
        static string TARGET_DIR;
        static string COMPLETE_DIR;
        
        public static void StartBuildProcess()
        {
            SetAndroidSDKPaths();
            SetTargetDirectory();
            
            var platform = ParseBuildPlatform();
            PerformBuild(platform);
        }

        static void SetTargetDirectory()
        {
            TARGET_DIR = Directory.GetCurrentDirectory() + "/outputs";
            COMPLETE_DIR = GetDirectoryName();
        }

		static void PerformBuild(BuildPlatform platform)
        {
            var scenes = FindEnabledEditorScenes();
            
            var buildName = PlayerSettings.productName;

            string targetDirectory = string.Empty;
            BuildTargetGroup buildGroup = BuildTargetGroup.Standalone;
            BuildTarget buildTarget = BuildTarget.StandaloneWindows64;

            switch (platform)
            {
                case BuildPlatform.Windows:
                    targetDirectory = COMPLETE_DIR + "/" + buildName + ".exe";
                    buildGroup = BuildTargetGroup.Standalone;
                    buildTarget = BuildTarget.StandaloneWindows64;
                    break;
                
                case BuildPlatform.Android:
                    targetDirectory = COMPLETE_DIR + "/" + buildName + ".apk";
                    buildGroup = BuildTargetGroup.Android;
                    buildTarget = BuildTarget.Android;
                    break;
                    
            }
            
            GenericBuild(scenes, targetDirectory, buildGroup, buildTarget, BuildOptions.None);
        }
        
        private static string[] FindEnabledEditorScenes()
        {
            List<string> editorScenes = new List<string>();

            // 빌드 세팅에 등록된 모든 씬을 순회합니다.
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                // 체크박스가 켜져 있는(enabled) 씬만 빌드 대상에 추가합니다.
                if (scene.enabled)
                {
                    editorScenes.Add(scene.path);
                }
            }

            return editorScenes.ToArray();
        }
        
        static void GenericBuild(string[] scenes, string target_dir, BuildTargetGroup build_group, BuildTarget build_target, BuildOptions build_options)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(build_group, build_target);

            if (build_target == BuildTarget.Android)
                PerformAndroidResolve();
            
            // 빌드 결과 리포트를 받습니다.
            var report = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"빌드 성공: {summary.totalSize} bytes, 경로: {target_dir}");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.LogError($"빌드 실패! 에러 개수: {summary.totalErrors}");
            }
        }
        
        static string GetDirectoryName()
        {
            var buildName = GetArg("-BuildName");
            var date = buildName.Split('_');
            var folderName = date[2];

            if (Directory.Exists(TARGET_DIR) == false)
            {
                Directory.CreateDirectory(TARGET_DIR);
            }

            if (Directory.Exists(TARGET_DIR + "/" + folderName) == false)
            {
                Directory.CreateDirectory(TARGET_DIR + "/" + folderName);
            }

            return TARGET_DIR + "/" + folderName;
        }
        
        static BuildPlatform ParseBuildPlatform()
        {
            // 빌드 플랫폼 파싱
            var platformArg = GetArg("-CustomPlatform");
            
            if (Enum.TryParse(platformArg, out BuildPlatform platform))
                return platform;

            Debug.LogError($"Invalid build platform: {platformArg}. Defaulting to Windows.");
            return BuildPlatform.Windows;   //기본값
        }

        
        
        static string GetArg(string name)
        {
            string[] arguments = Environment.GetCommandLineArgs();

            for (int nIndex = 0; nIndex < arguments.Length; ++nIndex)
            {
                if (arguments[nIndex] == name && arguments.Length > nIndex + 1)
                {
                    Debug.LogError(name + ", " + arguments[nIndex + 1]);

                    return arguments[nIndex + 1];
                }
            }

            Debug.LogError(string.Format("not found arg '{0}'", name));

            return null;
        }


        static void SetAndroidSDKPaths()
        {
            //Gradle 생성 위치 지정
            Environment.SetEnvironmentVariable("GRADLE_USER_HOME", @"C:\.gradle");
            
            // 1. 젠킨스 환경 변수를 최우선으로 가져옵니다.
            string envJdk = Environment.GetEnvironmentVariable("JAVA_HOME");
            string envSdk = Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT");
            string envNdk = Environment.GetEnvironmentVariable("ANDROID_NDK_HOME");

            // 2. 환경 변수 값이 있을 때만 EditorPrefs를 갱신합니다.
            if (!string.IsNullOrEmpty(envJdk)) EditorPrefs.SetString("JdkRoot", envJdk);
            if (!string.IsNullOrEmpty(envSdk)) EditorPrefs.SetString("AndroidSdkRoot", envSdk);
            if (!string.IsNullOrEmpty(envNdk)) EditorPrefs.SetString("AndroidNdkRoot", envNdk);

            // 최종적으로 유니티가 사용 중인 값을 출력합니다.
            Debug.LogError($"[Jenkins Sync] Current JDK: {EditorPrefs.GetString("JdkRoot")}");
            Debug.LogError($"[Jenkins Sync] Current SDK: {EditorPrefs.GetString("AndroidSdkRoot")}");
        }
        
        static void PerformAndroidResolve()
        {
            Debug.Log("[Jenkins] Android Resolver를 강제 실행합니다...");

            // 1. 유니티가 현재 프로젝트의 모든 에셋(DLL 포함)을 다시 읽게 합니다.
            AssetDatabase.Refresh();

            // 2. [가장 중요] 에디터 메뉴에 있는 'Force Resolve' 기능을 직접 실행합니다.
            bool success = EditorApplication.ExecuteMenuItem("Assets/External Dependency Manager/Android Resolver/Force Resolve");

            if (success)
            {
                Debug.Log("[Jenkins] Android Resolver 호출 성공!");
            }
            else
            {
                Debug.LogError("[Jenkins] 리졸버 메뉴를 찾을 수 없습니다. 경로를 확인하세요.");
            }
        }
        
    }
    
    
}












