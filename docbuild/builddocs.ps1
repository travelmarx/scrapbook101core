write-host "## Start of build script."
choco install docfx -y
docfx --help
write-host "## Location is" $PSScriptRoot.ToString()
