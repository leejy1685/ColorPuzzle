pipeline {
    agent any

    options {
        skipDefaultCheckout() 
        timeout(time: 1, unit: 'HOURS') 
        timestamps() 
    }

    environment {
        // 이미지 525323번에 적힌 설치 경로를 그대로 입력합니다.
        UNITY_EXE = 'C:\\Program Files\\Unity 2022.3.17f1\\Editor\\Unity.exe'
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
                // 플러그인 도구 이름(Unity_3.17f1) 대신 실제 실행 파일 경로를 사용하여 호출합니다.
                bat """
                    "${UNITY_EXE}" -quit -batchmode -nographics -projectPath . -executeMethod Editor.BuildScript.BuildWindows -logFile -
                """
            }
        }

        stage('GitHub Release') {
            steps {
                // 압축 파일 생성
                bat 'powershell "Compress-Archive -Path Builds\\MyGame\\* -DestinationPath ColorPuzzle.zip -Force"'
        
                // 공식 문서 규격에 맞춘 githubRelease 호출
                githubRelease(
                    id: 'github-token',      // image_50986c에 등록된 Credentials ID
                    tagName: 'latest',
                    releaseName: "Build #${env.BUILD_NUMBER}",
                    artifactFolder: '.',     // 현재 워크스페이스 기준
                    artifactScreenshots: 'ColorPuzzle.zip' // 업로드할 파일명
                )
            }
        }
    }
}