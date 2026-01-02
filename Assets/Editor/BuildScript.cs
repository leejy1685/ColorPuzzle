using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    
    public class BuildScript
    {
        static string TARGET_DIR;
        static string COMPLETE_DIR;
        
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
            BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
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












