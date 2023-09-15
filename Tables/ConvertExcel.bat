@SET EXCEL_FOLDER=.\Excel
@SET JSON_FOLDER=.\Json
@SET CSHARP_FOLDER=.\CSharp
@SET EXE=.\_excel2json\excel2json.exe

@ECHO Converting excel files in folder %EXCEL_FOLDER% ...
for /f "delims=" %%i in ('dir /b /a-d /s %EXCEL_FOLDER%\*.xlsx') do (
    @echo   processing %%~nxi 
    @CALL %EXE% --excel %EXCEL_FOLDER%\%%~nxi --json %JSON_FOLDER%\%%~ni.json --csharp %CSHARP_FOLDER%\%%~ni.cs --header 3
)