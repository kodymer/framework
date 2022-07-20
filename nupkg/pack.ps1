. ".\common.ps1"

# Rebuild all solutions
foreach($solution in $solutions) {
    $solutionFolder = Join-Path $rootFolder $solution
    Set-Location $solutionFolder
    & dotnet clean
    & dotnet restore
}

# Delete old packages
Remove-Item *.nupkg

# Create all packages
foreach($project in $projects) {
    
    $projectFolder = Join-Path $rootFolder $project
   
    # Create nuget pack
    Set-Location $projectFolder
    
    ###########################################################################################################

    dotnet msbuild -property:Configuration=Release -property:SourceLinkCreate=false
    & dotnet pack -c Release --no-build --include-symbols
    
    ###########################################################################################################

    if (-Not $?) {
        Write-Host ("Packaging failed for the project: " + $projectFolder)
        exit $LASTEXITCODE
    }
    
    # Copy nuget package
    $projectName = $project.Substring($project.LastIndexOf("/") + 1)

    $projectPackPath = Join-Path $projectFolder ("/bin/Release/" + $projectName + ".*.nupkg")
    Move-Item $projectPackPath $packFolder

}

# Go back to the pack folder
Set-Location $packFolder