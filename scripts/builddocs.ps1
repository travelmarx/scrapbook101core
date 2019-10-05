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
    Write-Host "## pwd:" (pwd).Path    
    $files = Get-ChildItem -Path .\docbuild\_site
    foreach ($f in $files)
    {
        Wrte-Host "## ## Copying" $f.FullName
        Copy-Item $f.FullName -Destination .\docs -Recurse -Force
    }
    
    Write-Host "## Copy files. Second step: copy complete."
    $directoryInfo = Get-ChildItem .\docs | Measure-Object
    if ($directoryInfo.Count -ne 0)
    {
        Write-Host "There is content to add to repo."
        Write-Host "Check in new content in docs folder."
    }
    
    Write-Host "## Check git command."
    git version

    Write-Host "## Build ran successfully."
}
catch {
    Write-Host "## Something went wrong in the build. Check the output."
}
