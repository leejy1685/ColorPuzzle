pipeline {
    agent any

    options {
        skipDefaultCheckout() 
        timeout(time: 1, unit: 'HOURS') 
        timestamps() 
    }

    stages {
        stage('Checkout') {
            steps {
                cleanWs() 
                checkout scm
            }
        }

        stage('Unity Windows Build') {
            steps {
                // 플러그인 공식 가이드에 따른 파이프라인 호출 방식입니다
                step([
                    $class: 'Unity3dBuilder',
                    unity3dName: 'Unity_3.17f1', // 젠킨스 전역 도구 설정에 등록하신 이름
                    argLine: '-quit -batchmode -nographics -projectPath . -executeMethod Editor.BuildScript.BuildWindows -logFile -'
                ])
            }
        }

        stage('GitHub Release Plugin') {
            steps {
                bat 'powershell "Compress-Archive -Path Builds\\MyGame\\* -DestinationPath ColorPuzzle.zip -Force"'

                // 플러그인 호출 시에도 인증 정보를 명시하는 것이 좋습니다.
                githubRelease(
                    // 만약 플러그인이 자동으로 environment의 GH_TOKEN을 못 읽는다면
                    // 아래와 같이 ID를 직접 지정해 보세요.
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