using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public enum AssetPackageType
{
    FullPack,      // 모든 씬을 포함하는 전체 빌드
    BundlesPack,   // 코드 로직에 따라 2번 인덱스까지만 포함하는 빌드
}


namespace Editor
{
    
    public class BuildScript
    {
        static string TARGET_DIR;
        static string COMPLETE_DIR;
        //아무것도 없음.
        
        public static void StartBuildProcess()
        {
            SetTargetDirectory();
            PerformWindowsBuild();
        }

        static void SetTargetDirectory()
        {
            TARGET_DIR = Directory.GetCurrentDirectory() + "/outputs";
            COMPLETE_DIR = GetDirectoryName();
        }

		static void PerformWindowsBuild()
        {
            var buildName = PlayerSettings.productName;
            var targetDirectory = COMPLETE_DIR + "/" + buildName + ".exe";
            var scenes = FindEnabledEditorScenes();

            GenericBuild(scenes, targetDirectory, BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64, BuildOptions.None);
        }
        
        private static string[] FindEnabledEditorScenes()
        {
            List<string> editorScenes = new List<string>();
            EditorBuildSettingsScene[] ebssArray = new EditorBuildSettingsScene[EditorBuildSettings.scenes.Length];
            var assetPackage = (AssetPackageType)Enum.Parse(typeof(AssetPackageType), GetArg("AssetPackage"));
            var sceneOrder = assetPackage == AssetPackageType.BundlesPack ? 2 : EditorBuildSettings.scenes.Length - 1;

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                EditorBuildSettingsScene curScene = EditorBuildSettings.scenes[i];
                bool enableScene = i <= sceneOrder;
                curScene.enabled = enableScene;
                ebssArray[i] = curScene;

                if (enableScene)
                {
                    editorScenes.Add(curScene.path);
                }
            }
            EditorBuildSettings.scenes = ebssArray;
            return editorScenes.ToArray();
        }
        
        static void GenericBuild(string[] scenes, string target_dir, BuildTargetGroup build_group, BuildTarget build_target, BuildOptions build_options)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(build_group, build_target);
            var report = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
            
            // Unity 6+ BuildReport is never null, check result instead
            if (report != null && report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build Result - " + report.summary.result + "\n" +
                          "Build Start - " + report.summary.buildStartedAt + "\n" +
                          "Build End - " + report.summary.buildEndedAt + "\n" +
                          "Build Size - " + report.summary.totalSize + "\n" +
                          "Build Error Count - " + report.summary.totalErrors);
            }
            else if (report != null)
            {
                Debug.LogError("Build Failed! Result: " + report.summary.result + 
                               "\nError Count: " + report.summary.totalErrors);
            }
        }
        
        static string GetDirectoryName()
        {
            var buildName = GetArg("BuildName");
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


    }
}












