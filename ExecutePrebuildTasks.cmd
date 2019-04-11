@ECHO off
SET SOLUTION_DIR=%1
SET PROJECT_DIR=%2

@ECHO on
@ECHO SOLUTION_DIR=%SOLUTION_DIR%
@ECHO PROJECT_DIR=%PROJECT_DIR%
@ECHO off

::Search $/Vueling/BuildProcessTemplates/NuGet directory
SET REL_PATH_Development=..\..\..\
::save current directory
pushd .
::change to relative directory and save value of CD (current directory) variable
cd %REL_PATH_Development%
SET ABS_PATH=%CD%
::restore current directory
popd

SET ensureNugetPacakgesPartialPack=BuildProcessTemplates\NuGet\EnsureNugetPackages.cmd

IF EXIST "%ABS_PATH%\%ensureNugetPacakgesPartialPack%" (
		ECHO Using Development
		SET BASE_DIR=%ABS_PATH%\
	) else 	(
		ECHO Using Continuous integration	
		SET BASE_DIR=%SOLUTION_DIR%
	)

SET ensureNugetPackagesFilePath=%BASE_DIR%%ensureNugetPacakgesPartialPack% 
SET nugetDirectory=%BASE_DIR%BuildProcessTemplates\NuGet\

::restore current directory
popd
@echo Relative path : %REL_PATH%
@echo Maps to path  : %ABS_PATH%


@ECHO on
@ECHO nugetDirectory=%nugetDirectory%
@ECHO ensureNugetPackagesFilePath=%ensureNugetPackagesFilePath%
@ECHO off

@ECHO on
@ECHO SOLUTION_DIR=%SOLUTION_DIR%
@ECHO PROJECT_DIR=%PROJECT_DIR%
@ECHO off

IF DEFINED ISTEAMBUILDMACHINE (
		"%ensureNugetPackagesFilePath%" %SOLUTION_DIR% %PROJECT_DIR% "%nugetDirectory%" 1> %PROJECT_DIR%..\..\Binaries\EnsureNugetPackages.log 2> %PROJECT_DIR%..\..\Binaries\EnsureNugetPackages.error
	) ELSE (
		"%ensureNugetPackagesFilePath%" %SOLUTION_DIR% %PROJECT_DIR% "%nugetDirectory%" 1> %PROJECT_DIR%EnsureNugetPackages.log 2> %PROJECT_DIR%EnsureNugetPackages.error
	)