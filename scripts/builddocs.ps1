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
    Set-Location -Path $PSScriptRoot
    Get-ChildItem .\docs -Recurse | Remove-Item -Recurse

    Write-Host "## Copy files. Second step: copy."
    $files = Get-ChildItem -Path .\docbuild\_site
    foreach ($f in $files)
    {
        Copy-Item $f.FullName -Destination .\docs -Recurse -Force
    }
    
    $directoryInfo = Get-ChildItem .\docs | Measure-Object
    if ($directoryInfo.Count -ne 0)
    {
        Write-Host "There is content to add to repo."
        Write-Host "Check in new content in docs folder."
    }
    
    Write-Host -ForegroundColor Green "## Build ran successfully."
}
catch {
    Write-Host -ForegroundColor Red "Something went wrong in the build. Check the output."
}
