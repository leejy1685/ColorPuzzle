pipeline {
    agent any

    options {
        skipDefaultCheckout() 
        timeout(time: 1, unit: 'HOURS') 
        timestamps() 
    }

    environment {
        // Unity 설치 경로 설정
        UNITY_EXE = 'C:\\Program Files\\Unity 2022.3.17f1\\Editor\\Unity.exe'

        // Android SDK
        JAVA_HOME = 'C:\\tool\\jdk-11'
        ANDROID_SDK_ROOT = 'C:\\tool\\SDK'
        ANDROID_NDK_HOME = 'C:\\tool\\SDK\\ndk\\23.1.7779620'

        BUILD_DATE = "${new Date().format('yyyyMMdd')}"
        BUILD_NAME_ARG = "ColorPuzzle_Build_${BUILD_DATE}"
        BUILD_TARGET = "Android"
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
                script {
                    // 1. 유니티 호출 (StartBuildProcess 메서드 실행)
                    bat """
                        set JAVA_HOME=${env.JAVA_HOME}
                        set PATH=%JAVA_HOME%\\bin;%PATH%
                        set GRADLE_OPTS="-Dhttps.protocols=TLSv1.2,TLSv1.3"

                        "${UNITY_EXE}" -quit -batchmode -nographics -projectPath . \
                        -executeMethod Editor.BuildScript.StartBuildProcess \
                        -BuildName ${env.BUILD_NAME_ARG} \
                        -CustomPlatform ${env.BUILD_TARGET} \
                        -logFile -
                    """
                }
            }
        }

        stage('GitHub Release') {
            steps {
                script {
                    // 3. 유니티 스크립트의 COMPLETE_DIR(/outputs/날짜) 경로를 압축
                    // COMPLETE_DIR 내부의 결과물들을 zip 하나로 묶습니다.
                    bat "powershell \"Compress-Archive -Path outputs/${env.BUILD_DATE}/* -DestinationPath ColorPuzzle.zip -Force\""

                    // 4. GitHub CLI를 사용한 릴리스 생성 및 파일 업로드
                    // --generate-notes: 커밋 내역 자동 생성
                    withCredentials([string(credentialsId: 'github-token', variable: 'GH_TOKEN')]) {
                        bat """
                            gh release delete latest -y --cleanup-tag || echo "No existing release"
                            
                            gh release create latest ColorPuzzle.zip ^
                            --title "Build #${env.BUILD_NUMBER} (${env.BUILD_DATE})" ^
                            --generate-notes
                        """
                    }
                }
            }
        }
    }
}