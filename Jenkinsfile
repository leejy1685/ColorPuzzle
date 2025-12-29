pipeline {
    agent any

    options {
        skipDefaultCheckout() 
        timeout(time: 1, unit: 'HOURS') 
        timestamps() 
    }

    environment {
        // 젠킨스 도구 설정에 등록된 유니티 경로를 자동으로 가져옵니다.
        UNITY_HOME = tool name: 'Unity_3.17f1', type: 'com.horstmann.jenkins.plugins.unity3d.Unity3dInstallation'
        GH_TOKEN = credentials('github-token')
    }

    stages {
        stage('Checkout') {
            steps {
                cleanWs() 
                checkout scm
            }
        }

        stage('Unity Build') {
            steps {
                // 플러그인 클래스 대신 툴 경로를 이용해 직접 실행합니다.
                bat """
                    "${env.UNITY_HOME}\\Editor\\Unity.exe" -quit -batchmode -nographics -projectPath . -executeMethod Editor.BuildScript.BuildWindows -logFile -
                """
            }
        }

        stage('GitHub Release') {
            steps {
                bat 'powershell "Compress-Archive -Path Builds\\MyGame\\* -DestinationPath ColorPuzzle.zip -Force"'
                
                githubRelease(
                    credentialsId: 'github-token',
                    tagName: 'latest',
                    releaseName: "Latest Build (#${env.BUILD_NUMBER})",
                    artifacts: 'ColorPuzzle.zip',
                    overwrite: true
                )
            }
        }
    }
}