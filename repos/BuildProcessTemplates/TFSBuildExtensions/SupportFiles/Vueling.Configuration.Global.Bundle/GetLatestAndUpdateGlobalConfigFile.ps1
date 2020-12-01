### Declared variables ###

$tf = "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\tf.exe"

### Declared variables ###


Function GetLatestConfigurationSolution($globalConfigDirectoryPath)
{
	& $tf get $globalConfigDirectoryPath /recursive
}

Function BundleGlobalConfigFile($globalConfigDirectoryPath)
{
	$ret = .\Vueling.Configuration.Global.Bundle.ConsoleUI.exe $globalConfigDirectoryPath
}

if ($args.Length -ne 1)
{
	Write-Host "Incorrect params! Usage: GetLatestUpdateGlobalConfigFile.ps1 <path global directory>"
	exit 1
}
else
{
	$pGlobalConfigDirectoryPath = $args[0]
	
	GetLatestConfigurationSolution($pGlobalConfigDirectoryPath)
	BundleGlobalConfigFile($pGlobalConfigDirectoryPath)
}