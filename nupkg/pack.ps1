. ".\common.ps1"

# Delete old packages
Remove-Item *.nupkg

# Count


# Rebuild all solutions
foreach($solution in $solutions) {

    Write-Host $solution.BasePath

    if(-Not ($null -eq $solution.Solution )) {
        $solutionFolder = Join-Path $rootFolder $solution.BasePath
        Set-Location $solutionFolder
        & dotnet restore
    }
    
    $i = 0;
    $projectsCount = $solution.Projects.Count

    # Create all packages
    foreach($project in $solution.Projects) {
        
        $i += 1;

        $projectFolder = Join-Path $rootFolder $solution.BasePath $project
        $projectName = ($project -split '/')[-1]
        
        Write-Host $projectFolder 

        # Create nuget pack
        Write-Host "[$i / $projectsCount] - Packing project: $projectName"
        Set-Location $projectFolder
        
        dotnet clean -c Release
        dotnet build -c Release
        dotnet pack -c Release --no-build --no-restore

        if (-Not $?) {
            Write-Host ("Packaging failed for the project: " + $projectFolder)
            exit $LASTEXITCODE
        }
    
        # Copy nuget package        
        $projectPackPath = Join-Path $projectFolder ("/bin/Release/" + $projectName + ".*.nupkg")
        Move-Item $projectPackPath $packFolder
        
    }
}

# Go back to the pack folder
Set-Location $packFolder
