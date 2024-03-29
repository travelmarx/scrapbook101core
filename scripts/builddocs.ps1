#
# A script for building Scrapbook101core docs in a GitHub Actions Workflow.
# See the .github\workflows folder
#

Write-Host "## Redirect stderr to stdout."
$env:GIT_REDIRECT_STDERR = '2>&1'

Write-Host "## Config username and email."
git config --global user.email "travelmarx@live.com"
git config --global user.name "Travelmarx"

Write-Host "## Start of build script."
Write-Host "## Script location is" $PSScriptRoot.ToString()
try {
    write-host "## Install chocolatey."
    choco install docfx -y

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
        
        Write-Host "## Check git version: git --version"
        git --version

        Write-Host "## Check git branch"
        git branch

        Write-Host "## Staging files: git status"
        git status
        
        Write-Host "## git checkout -b tmp"
        git checkout -b tmp

        Write-Host "## Check git branch to list"
        git branch

        Write-Host "## git config core.autocrlf true"
        git config core.autocrlf true

        Write-Host "## Location is" (Get-Location).Path

        Write-Host "## git add"
        git add . --ignore-errors

        Write-Host "## git commit -a m'message'"
        git commit -m"[skip ci]Action build check in."

        Write-Host "## Switch to main: git checkout main"
        git checkout main

        Write-Host "## Add remote: git remote add origin main url"
        git remote add origin https://github.com/travelmarx/scrapbook101core.git

        Write-Host "## Pull from origin: git pull origin main"
        git pull origin main

        Write-Host "## git merge tmp"
        git merge tmp

        Write-Host "## git push"
        git push
        }    
 
    Write-Host "## Build ran successfully."
}
catch {
    Write-Host "## Something went wrong in the build. Check the output."
}
