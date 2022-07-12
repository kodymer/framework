# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

$framework = [PSCustomObject]@{
    BasePath = "framework"
    Solution = "Vesta.Framework"
    Projects = (
        "src/Vesta.ApplicationInsights.AspNetCore",
        "src/Vesta.AspNetCore",
        "src/Vesta.AspNetCore.Mvc",
        "src/Vesta.Auditing",
        "src/Vesta.Auditing.Abstracts",
        "src/Vesta.Autofac",
        "src/Vesta.AutoMapper",
        "src/Vesta.Caching",
        "src/Vesta.Caching.StackExchangeRedis",
        "src/Vesta.Core",
        "src/Vesta.Dapper",
        "src/Vesta.Dapper.SqlServer",
        "src/Vesta.Data",
        "src/Vesta.Ddd.Application",
        "src/Vesta.Ddd.Domain",
        "src/Vesta.Ddd.Domain.EventBus",
        "src/Vesta.EntityFrameworkCore",
        "src/Vesta.EntityFrameworkCore.Abstracts",
        "src/Vesta.EntityFrameworkCore.SqlServer",
        "src/Vesta.EventBus",
        "src/Vesta.EventBus.Abstracts",
        "src/Vesta.EventBus.Azure",
        "src/Vesta.Security",
        "src/Vesta.ServiceBus.Abstracts",
        "src/Vesta.ServiceBus.Azure",
        "src/Vesta.ServiceBus.Local",
        "src/Vesta.TestBase",
        "src/Vesta.Uow"
    )
  }

$templates = [PSCustomObject]@{
    BasePath = "templates"
    Solution = $null
    Projects = (
        "src/Vesta.Templates"
    )
  }


# List of solutions
$solutions = (
    $framework,
    $templates
)