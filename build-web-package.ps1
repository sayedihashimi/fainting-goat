# This script can be used to build the MSDeploy package for fainting-goat.
# This package can be imported into IIS/Windows Azure to create a site easily.


function Get-ScriptDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
    Split-Path $Invocation.MyCommand.Path
}

$pathToCsProj = Join-Path (Get-ScriptDirectory) fainting-goat\fainting-goat.csproj
"pathToCsProj: {0}" -f $pathToCsProj | Write-Output

$msbuild =  "{0}\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" -f $env:windir

& $msbuild "$pathToCsProj" /p:DeployOnBuild=true /p:VisualStudioVersion=11.0 /p:PublishProfile=ToPkg /flp:logfile=msbuild.pkg.log


