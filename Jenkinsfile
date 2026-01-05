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
        // Jenkins에 만든 권한과 이름이 일치해야함.
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
                    // 1. 빌드 씬 설정
                    def packageType = "FullPack"

                    // 2. 유니티 호출 (StartBuildProcess 메서드 실행)
                    // 내부에서 SetTargetDirectory()와 PerformWindosBuild()가 모두 실행됩니다.
                    bat """
                        "${UNITY_EXE}" -quit -batchmode -nographics -projectPath . \
                        -executeMethod Editor.BuildScript.StartBuildProcess \
                        -BuildName ${env.BUILD_NAME_ARG} \
                        -BuildTarget ${env.BUILD_TARGET}
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