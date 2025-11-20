@echo off
REM اسکریپت Batch برای اجرای برنامه با تنظیمات UTF-8
chcp 65001 >nul
echo اجرای برنامه نمونه Pub-Sub...
echo.
dotnet run --project PubSub.Example/PubSub.Example.csproj
pause

