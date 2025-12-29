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
                // 업로드할 파일을 먼저 압축합니다.
                bat 'powershell "Compress-Archive -Path Builds\\MyGame\\* -DestinationPath ColorPuzzle.zip -Force"'
        
                // 3. 플러그인에 삭제 기능이 없으므로 gh 명령어로 기존 'latest'를 먼저 지워줍니다.
                withCredentials([string(credentialsId: 'github-token', variable: 'GH_TOKEN')]) {
                    bat 'gh release delete latest -y --cleanup-tag || echo "No existing release"'
                }

                createGitHubRelease(
                    credentialId: 'github-token',
                    githubServer: 'https://api.github.com',
                    repository: 'leejy1685/ColorPuzzle',
                    tag: 'latest',
                    name: "Build #${env.BUILD_NUMBER}",
                    bodyText: '자동 빌드 배포',
                    commitish: 'main',
                    draft: false,
                    prerelease: false,
                )

                // 4. 플러그인이 못하는 파일 업로드를 gh 명령어로 마무리
                withCredentials([string(credentialsId: 'github-token', variable: 'GH_TOKEN')]) {
                    bat 'gh release upload latest ColorPuzzle.zip --clobber'
                }
            }
        }
    }
}