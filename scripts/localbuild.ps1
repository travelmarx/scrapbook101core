﻿#
# A script for building Scrapbook101core docs locally.
# Start in PowerShell.
#
# Assumes docfx is available in the system.
# https://dotnet.github.io/docfx/
#
# Issues with permissions running the script, see:
# See https://learn.microsoft.com/powershell/module
# /microsoft.powershell.core/about/about_execution_policies

Write-Host "## Redirect stderr to stdout."
$env:GIT_REDIRECT_STDERR = '2>&1'

Write-Host "## Start of local build script."
Write-Host "## Script location is" $PSScriptRoot.ToString()

try {
    Write-Host "## Run DocFx."
    Write-Host "## Location is" (Get-Location).Path
    Set-Location -Path "docbuild"
    Write-Host "## Location is" (Get-Location).Path

    docfx metadata
    docfx build

    Write-Host "## Docfx ran successfully."

    Write-Host "## Copy files. First step: delete old."
    Set-Location -Path ".."
    Write-Host "## Location is" (Get-Location).Path
    Get-ChildItem .\docs -Recurse | Remove-Item -Recurse

    Write-Host "## Copy files. Second step: start copy."
    $files = Get-ChildItem -Path .\docbuild\_site
    Write-Host "## Files count =" $files.Count
    foreach ($f in $files)
    {
        Write-Host "## ## Copying" $f.FullName
        Copy-Item $f.FullName -Destination .\docs -Recurse -Force
    }
    
    Write-Host "## Copy files. Second step: copy complete."
    $directoryInfo = Get-ChildItem .\docs | Measure-Object
    if ($directoryInfo.Count -ne 0)
    {
        Write-Host "## There is content to add to repo."
        Write-Host "## Checking in changes."
        git status
        git add .
        git commit -m"Local build check in. [skip ci]"
        git push
    }
 
    Write-Host "## Build ran successfully."
}
catch {
    Write-Host "## Something went wrong in the build. Check the output."

}
