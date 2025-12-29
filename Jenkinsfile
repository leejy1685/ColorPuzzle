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
                // 빌드 결과물 압축
                bat 'powershell "Compress-Archive -Path Builds\\MyGame\\* -DestinationPath ColorPuzzle.zip -Force"'
        
                // Credentials ID 'github-token'을 사용하여 gh cli로 직접 배포
                withCredentials([string(credentialsId: 'github-token', variable: 'GH_TOKEN')]) {
                    bat """
                        gh release delete latest -y --cleanup-tag || echo "No existing release"
                        gh release create latest ColorPuzzle.zip --title "Build #${env.BUILD_NUMBER}" --notes "Automated build"
                    """
                }
            }
        }
    }
}