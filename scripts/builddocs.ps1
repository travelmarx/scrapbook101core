#
# A script for building Scrapbook101core docs in an Azure pipeline.
#
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
    Write-Host "## git checkout -b tmp"
    git checkout -q -b tmp

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
        git checkout master
        #git branch tmp head
        git merge tmp
        git commit -m"Pipeline build check in."
        git push
        }
    
 
    Write-Host "## Build ran successfully."
}
catch {
    Write-Host "## Something went wrong in the build. Check the output."
}
