Write-Host "## Start of build script."
Write-Host "## Location is" $PSScriptRoot.ToString()
try {
    write-host "## Install chocolatey."
    choco install docfx -y

    Write-Host "## Run DocFx."
    Write-Host "## Location is" $PSScriptRoot.ToString()

    docfx metadata
    docfx build

    Write-Host "## Docfx ran successfully."

    Write-Host "## Copy files."
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
catch {
    Write-Host -ForegroundColor Red "Something went wrong in the build. Check the output."
}
