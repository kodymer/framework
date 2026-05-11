# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

$framework = [PSCustomObject]@{
    BasePath = "framework"
    Solution = "CompanyName.Framework"
    Projects = (
    )
  }

$templates = [PSCustomObject]@{
    BasePath = "templates"
    Solution = $null
    Projects = (
        "src/CompanyName.Templates"
    )
}

$modules = [PSCustomObject]@{
    BasePath = "modules"
    Solution = $null
    Projects = (
    )
}