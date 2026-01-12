@echo off
:: 관리자 권한으로 실행 중인지 확인
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo 관리자 권한이 필요합니다. '예'를 눌러주세요...
    powershell start-process -FilePath '%0' -Verb RunAs
    exit /b
)

echo Gradle 캐시를 삭제 중입니다...
:: 질문하신 코드 실행
rd /s /q "C:\Windows\System32\config\systemprofile\.gradle\caches"

echo.
echo [삭제 완료] 이제 젠킨스에서 빌드를 다시 시도하세요.
timeout /t 3