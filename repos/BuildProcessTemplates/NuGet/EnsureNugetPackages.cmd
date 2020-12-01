@ECHO off
SET SOLUTION_DIR=%1
SET SOLUTION_DIR=%SOLUTION_DIR:"=%
SET PROJECT_DIR=%2
SET PROJECT_DIR=%PROJECT_DIR:"=%
SET NUGET_DIR=%3
SET NUGET_DIR=%NUGET_DIR:"=%

@ECHO ON
@ECHO Initial parameters
@ECHO SOLUTION_DIR=%SOLUTION_DIR%
@ECHO PROJECT_DIR=%PROJECT_DIR%
@ECHO NUGET_DIR=%NUGET_DIR%
@ECHO off

@ECHO on
@ECHO Getting environment

SET INT=INT
SET PRE=PRE
SET CMS=CMS
SET TEST=TEST

@ECHO if no environment detected set default environment
SET environment=%INT%

::Get the current environment
::delete INT from PROJECT_DIR for later comparison
SET INTEnvironmentStringChecker=%PROJECT_DIR:INT=% 
::delete PRE from PROJECT_DIR for later comparison
SET PREEnvironmentStringChecker=%PROJECT_DIR:PRE=% 
::delete PRO from PROJECT_DIR for later comparison
SET PROEnvironmentStringChecker=%PROJECT_DIR:.PRO.=% 
::delete CMSPRO from PROJECT_DIR for later comparison
SET CMSPROEnvironmentStringChecker=%PROJECT_DIR:.PRO.64\Sources\MustDirToFindNuget\Vueling.Corporative=% 
::delete TESTREPOSITORY from PROJECT_DIR for later comparison
SET TESTREPOSITORYEnvironmentStringChecker=%PROJECT_DIR:TestRepository=%


IF NOT %INTEnvironmentStringChecker%==%PROJECT_DIR% (SET environment=%INT%) 
IF NOT %PREEnvironmentStringChecker%==%PROJECT_DIR% (SET environment=%INT%) 
IF NOT %PROEnvironmentStringChecker%==%PROJECT_DIR% (SET environment=%INT%) 
IF NOT %CMSPROEnvironmentStringChecker%==%PROJECT_DIR% (SET environment=%CMS%) 
IF NOT %TESTREPOSITORYEnvironmentStringChecker%==%PROJECT_DIR% (SET environment=%TEST%)

@ECHO on
@ECHO environment=%environment%
@ECHO off

@ECHO ON
@ECHO Environment variables Checking
@ECHO SOLUTION_DIR=%SOLUTION_DIR%
@ECHO PROJECT_DIR=%PROJECT_DIR%
@ECHO NUGET_DIR=%NUGET_DIR%
@ECHO off

@ECHO on
@ECHO Getting nuget server
@ECHO off

SET nugetServerIntegrationFilePath=%NUGET_DIR%NugetServeIntegrationAddress.txt
SET nugetServerPreproductionFilePath=%NUGET_DIR%NugetServePreproductionAddress.txt
SET nugetServerCMSPROFilePath=%NUGET_DIR%NugetServerCMSPROAddress.txt
SET nugetServerTestRepositoryFilePath=%NUGET_DIR%NugetServerTestRepositoryAddress.txt

IF %environment% ==  %INT% (SET environmentAddressPath="%nugetServerIntegrationFilePath%")
IF %environment% ==  %PRE% (SET environmentAddressPath="%nugetServerPreproductionFilePath%")
IF %environment% ==  %CMS% (SET environmentAddressPath="%nugetServerCMSPROFilePath%")
IF %environment% ==  %TEST% (SET environmentAddressPath="%nugetServerTestRepositoryFilePath%")

@ECHO on
@ECHO environmentAddressPath=%environmentAddressPath%
@ECHO off

FOR /f "usebackq delims=" %%a IN (%environmentAddressPath%) DO (
  SET nuget_server=%%a
)

@ECHO on
@ECHO nuget_server=%nuget_server%
@ECHO off

SET packagesConfigPath="%PROJECT_DIR%packages.config"
SET nugetFilePath="%NUGET_DIR%NuGet"
SET packagesDirectoryPath="%SOLUTION_DIR%packages"

@ECHO on
@ECHO packagesConfigPath=%packagesConfigPath%
@ECHO nugetFilePath=%nugetFilePath%
@ECHO packagesDirectoryPath=%packagesConfigPath%
@ECHO off


IF EXIST %packagesConfigPath% (
	%nugetFilePath% install %packagesConfigPath% -o %packagesDirectoryPath% -Source %nuget_server% -NoCache
) ELSE (
	ECHO NO packages.config file found at %PROJECT_DIR%. NuGet was not executed.
)