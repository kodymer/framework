. ".\common.ps1"

# Get the version
[xml]$commonPropsXml = Get-Content (Join-Path $rootFolder "common.props")
$version = $commonPropsXml.Project.PropertyGroup.Version

$v = "$($version)".Trim(' ')
# Publish all packages
foreach($project in $projects) {
    $projectName = $project.Substring($project.LastIndexOf("/") + 1)
    # Write-host "$($projectName).$($v).nupkg"
    & dotnet nuget push "$($projectName).$($v).nupkg"--source "HLA.Viena.Feed" --api-key az --skip-duplicate --interactive
}

# Go back to the pack folder
Set-Location $packFolder
