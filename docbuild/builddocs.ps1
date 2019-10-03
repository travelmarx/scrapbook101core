Write-Host -ForegroundColor Green "## Start of build script."
Write-Host -ForegroundColor Green "## Location is" $PSScriptRoot.ToString()
try {
    Write-Host -ForegroundColor Green "## Install chocolatey."
    choco install docfx -y

    Write-Host -ForegroundColor Green "## Run DocFx."
    docfx metadata
    docfx build
    Write-Host -ForegroundColor Green "## Docfx ran successfully."

    Write-Host -ForegroundColor Green "## Copy files."
    Get-ChildItem .\docs -Recurse | Remove-Item -Recurse

    $files = Get-ChildItem -Path .\docbuild\_site
    foreach ($f in $files)
    {
        Copy-Item $f.FullName -Destination .\docs -Recurse -Force
    }
    
    $directoryInfo = Get-ChildItem .\docs | Measure-Object
    if ($directoryInfo.Count -ne 0)
    {
        Write-Host "There is content to add to repo."
    }
    
    Write-Host -ForegroundColor Green "## Build ran successfully."
}
finally {
    Write-Host -ForegroundColor Red "Something went wrong in the build. Check the output."
}
