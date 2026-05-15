param(
    [switch]$template
)

. ".\common.ps1"

if ($template) {
    $solutions = @($templates)
} else {
    $solutions = @($framework, $modules)
}

# Get the version
[xml]$commonPropsXml = Get-Content (Join-Path $rootFolder "Directory.Build.props")
$version = $commonPropsXml.Project.PropertyGroup.Version

$v = "$($version)".Trim(' ')

foreach($solution in $solutions) {

# Publish all packages
    foreach($project in $solution.Projects) {
        $projectName = ($project -split '/')[-1]

        dotnet nuget push "$($projectName).$($v).nupkg" --source "CompanyNameLower.nuget.corp" --api-key az --skip-duplicate --interactive
        dotnet nuget push "$($projectName).$($v).snupkg" --source "CompanyNameLower.nuget.corp" --api-key az --skip-duplicate --interactive
    }
}

# Go back to the pack folder
Set-Location $packFolder


