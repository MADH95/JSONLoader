@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

REM -------------------------------
REM THIS SCRIPT IS PROVIDED BY CHAOSYR
REM THIS IS LICENSED UNDER SML-1.0.0
REM IT CAN BE FOUND HERE: https://stoatgames.icu/license-repository/licenses/sml-1-0-0/#content
REM IF YOU REUSE THIS FOR YOUR PROJECT, THIS COMMENT BLOCK MUST REMAIN IN TACT FOR LEGAL REASONS!!
REM -------------------------------

REM -------------------------------
REM Config
REM -------------------------------
set "CSProj=%~dp0JSONLoaderOld.csproj"

if not exist "%CSProj%" (
    echo ERROR: Project file not found: "%CSProj%"
    exit /b 1
)

REM -------------------------------
REM Read TargetFrameworks
REM -------------------------------
set "TFs="
for /f "usebackq tokens=*" %%A in (`type "%CSProj%" ^| findstr "<TargetFrameworks>"`) do (
    set "line=%%A"
    set "line=!line:<TargetFrameworks>=!"
    set "line=!line:</TargetFrameworks>=!"
    set "line=!line:;= !"
    for %%B in (!line!) do set "TFs=!TFs! %%B"
)
set "TFs=!TFs:~1!"

REM -------------------------------
REM Read RuntimeIdentifiers
REM -------------------------------
set "RIDs="
for /f "usebackq tokens=*" %%A in (`type "%CSProj%" ^| findstr "<RuntimeIdentifiers>"`) do (
    set "line=%%A"
    set "line=!line:<RuntimeIdentifiers>=!"
    set "line=!line:</RuntimeIdentifiers>=!"
    set "line=!line:;= !"
    for %%B in (!line!) do set "RIDs=!RIDs! %%B"
)
set "RIDs=!RIDs:~1!"

REM -------------------------------
REM Read Copy Locations
REM -------------------------------
set "CLs="
for /f "usebackq tokens=*" %%A in (`type "%CSProj%" ^| findstr "<BuildCopyLocations>"`) do (
    set "line=%%A"
    set "line=!line:<BuildCopyLocations>=!"
    set "line=!line:</BuildCopyLocations>=!"
    set "line=!line:;= !"
    for %%B in (!line!) do set "CLs=!CLs! %%B"
)
set "CLs=!CLs:~1!"

REM -------------------------------
REM Read Dependency Locations
REM -------------------------------
set "DPLs="
for /f "usebackq tokens=*" %%A in (`type "%CSProj%" ^| findstr "<DependenciesCopyLocations>"`) do (
    set "line=%%A"
    set "line=!line:<DependenciesCopyLocations>=!"
    set "line=!line:</DependenciesCopyLocations>=!"
    set "line=!line:;= !"
    for %%B in (!line!) do set "DPLs=!DPLs! %%B"
)
set "DPLs=!DPLs:~1!"

REM -------------------------------
REM Read Markdown File Locations
REM -------------------------------
set "CHANGELOG="
set "README="
set "LICENSE="

for /f "usebackq tokens=*" %%A in (`type "%CSProj%" ^| findstr "<CHANGELOGCopyLocation>"`) do (
    set "line=%%A"
    set "line=!line:<CHANGELOGCopyLocation>=!"
    set "line=!line:</CHANGELOGCopyLocation>=!"
    set "CHANGELOG=!line!"
)

for /f "usebackq tokens=*" %%A in (`type "%CSProj%" ^| findstr "<READMECopyLocation>"`) do (
    set "line=%%A"
    set "line=!line:<READMECopyLocation>=!"
    set "line=!line:</READMECopyLocation>=!"
    set "README=!line!"
)

for /f "usebackq tokens=*" %%A in (`type "%CSProj%" ^| findstr "<LICENSECopyLocation>"`) do (
    set "line=%%A"
    set "line=!line:<LICENSECopyLocation>=!"
    set "line=!line:</LICENSECopyLocation>=!"
    set "LICENSE=!line!"
)

REM -------------------------------
REM Read Release Mode
REM -------------------------------

set "RELEASEMODE="
for /f "usebackq tokens=*" %%A in (`type "%CSProj%" ^| findstr "<ReleaseMode>"`) do (
    set "line=%%A"
    set "line=!line:<ReleaseMode>=!"
    set "line=!line:</ReleaseMode>=!"
    set "RELEASEMODE=!line!"
)

if "!RELEASEMODE!"=="" set "RELEASEMODE=Debug"

if /I "!RELEASEMODE!"=="Release" (
    set "OUTPUT_DIR=%~dp0GITHUB RELEASE"
) else (
    if /I "!RELEASEMODE!"=="Debug" (
        set "OUTPUT_DIR=%~dp0GITHUB PRERELEASE"
    ) else (
        set "RELEASEMODE=Debug"
        set "OUTPUT_DIR=%~dp0GITHUB PRERELEASE"
    )
)

REM Normalize full path
for %%I in ("%OUTPUT_DIR%") do set "OUTPUT_DIR=%%~fI"

REM -------------------------------
REM Clean output root safely
REM -------------------------------
if exist "%OUTPUT_DIR%" rd /s /q "%OUTPUT_DIR%"
mkdir "%OUTPUT_DIR%"

mkdir "%OUTPUT_DIR%\NET"

set "TMP_ROOT=%OUTPUT_DIR%\_tmp"
mkdir "%TMP_ROOT%"

set "MANIFEST_ROOT=%OUTPUT_DIR%\_manifest"
mkdir "%MANIFEST_ROOT%"

REM -------------------------------
REM Tell User the Settings
REM -------------------------------

if "!TFs!"=="~1" set "TFs=net6.0"
if "!RIDs!"=="~1" set "RIDs=win-x64"
if "!CLs!"=="~1" set "CLs=Thunderstore-Package-Elements"
if "!DPLs!"=="~1" set "DPLs=Dependencies/BepInEx-Required-Dependencies"

echo CONFIGURATION
echo TargetFrameworks: !TFs!
echo RuntimeIdentifiers: !RIDs!
echo CopyLocations: !CLs!
echo DependencyLocations: !DPLs!
echo CHANGELOGCopyLocation: !CHANGELOG!
echo READMECopyLocation: !README!
echo LICENSECopyLocation: !LICENSE!
echo ReleaseMode: !RELEASEMODE!

REM -------------------------------
REM Fetch Manifest Properties
REM -------------------------------
set "NAME="
set "VERSION="
set "WEBSITEURL="
set "DESCRIPTION="
set "DEPENDENCIES="

for /f "usebackq tokens=*" %%A in (`type "%CSProj%" ^| findstr "<ThunderstorePackageName>"`) do (
    set "line=%%A"
    set "line=!line:<ThunderstorePackageName>=!"
    set "line=!line:</ThunderstorePackageName>=!"
    set "NAME=!line!"
)

for /f "usebackq tokens=*" %%A in (`type "%CSProj%" ^| findstr "<ThunderstorePackageVersion>"`) do (
    set "line=%%A"
    set "line=!line:<ThunderstorePackageVersion>=!"
    set "line=!line:</ThunderstorePackageVersion>=!"
    set "VERSION=!line!"
)

for /f "usebackq tokens=*" %%A in (`type "%CSProj%" ^| findstr "<ThunderstoreWebsiteURL>"`) do (
    set "line=%%A"
    set "line=!line:<ThunderstoreWebsiteURL>=!"
    set "line=!line:</ThunderstoreWebsiteURL>=!"
    set "WEBSITEURL=!line!"
)

for /f "usebackq tokens=*" %%A in (`type "%CSProj%" ^| findstr "<ThunderstorePackageDescription>"`) do (
    set "line=%%A"
    set "line=!line:<ThunderstorePackageDescription>=!"
    set "line=!line:</ThunderstorePackageDescription>=!"
    set "DESCRIPTION=!line!"
)

for /f "usebackq tokens=*" %%A in (`type "%CSProj%" ^| findstr "<ThunderstorePackageDependencyStrings>"`) do (
    set "line=%%A"
    set "line=!line:<ThunderstorePackageDependencyStrings>=!"
    set "line=!line:</ThunderstorePackageDependencyStrings>=!"
    set "line=!line:;= !"
    for %%B in (!line!) do set "DEPENDENCIES=!DEPENDENCIES! %%B"
)
set "DEPENDENCIES=!DEPENDENCIES:~1!"

if "!DEPENDENCIES!"=="~1" set "DEPENDENCIES="

set "DEPENDENCYCOUNT=0"

for %%D in (!DEPENDENCIES!) do (
    set /a DEPENDENCYCOUNT+=1
)

echo MANIFEST PROPERTIES
echo NAME: !NAME!
echo VERSION: !VERSION!
echo WEBSITEURL: !WEBSITEURL!
echo DESCRIPTION: !DESCRIPTION!
echo DEPENDENCIES: !DEPENDENCIES!
echo DEPENDENCYCOUNT: !DEPENDENCYCOUNT!

REM -------------------------------
REM Build Manifest File
REM -------------------------------

set "OUT=%MANIFEST_ROOT%\manifest.json"

(
    echo {
    echo    "name": "!NAME!",
    echo    "version_number": "!VERSION!",
    echo    "website_url": "!WEBSITEURL!",
    echo    "description": "!DESCRIPTION!",
    echo    "dependencies": [
) > "!OUT!"

if "!DEPENDENCYCOUNT!"=="0" (
    echo No Dependencies Skipping
) else (
    set "INDEX=0"
    
    for %%D in (!DEPENDENCIES!) do (
        set /a INDEX+=1
    
        if "!INDEX!"=="!DEPENDENCYCOUNT!" (
            echo         "%%D"
        ) else (
            echo         "%%D",
        )
    ) >> "!OUT!"
)

(
    echo    ]
    echo }
) >> "!OUT!"

REM -------------------------------
REM Build Loop
REM -------------------------------
for %%F in (!TFs!) do (
    for %%R in (!RIDs!) do (

        set "TF_NAME=%%F"
        set "RID_FLAT=%%R"
        set "OUT=!TMP_ROOT!\!TF_NAME!-!RID_FLAT!"

        if exist "!OUT!" rd /s /q "!OUT!"
        mkdir "!OUT!"

        echo Publishing NET: %%F / %%R
        dotnet publish "%CSProj%" -c "!RELEASEMODE!" -f "%%F" -r "%%R" -o "!OUT!" || echo Failed %%F/%%R
        copy /Y "!CHANGELOG!" "!OUT!"
        copy /Y "!README!" "!OUT!"
        copy /Y "!LICENSE!" "!OUT!"
        copy /Y "%MANIFEST_ROOT%\manifest.json" "!OUT!"
        mkdir "!OUT!\plugins"
        mkdir "!OUT!\plugins\Dependencies"
        for %%L in (!DPLs!) do (
            xcopy /E /I /Y "%%L" "!OUT!\plugins\Dependencies"
        )
        for %%L in (!CLs!) do (
            xcopy /E /I /Y "%%L" "!OUT!"
        )
        set "ZIP_OUT=%OUTPUT_DIR%\NET\!TF_NAME!-!RID_FLAT!-!RELEASEMODE!.zip"

        powershell -NoProfile -Command ^ "Compress-Archive -Path '!OUT!\*' -DestinationPath '!ZIP_OUT!' -Force"

        rd /s /q "!OUT!"
    )
)

REM -------------------------------
REM Remove temp root completely
REM -------------------------------
rd /s /q "%TMP_ROOT%"
rd /s /q "%MANIFEST_ROOT%"

echo.
echo Build completed! Artifacts in:
echo %OUTPUT_DIR%

ENDLOCAL