# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

$framework = [PSCustomObject]@{
    BasePath = "framework"
    Solution = "CompanyName.Framework"
    Projects = (
        "src/CompanyName.ApplicationInsights.AspNetCore",
        "src/CompanyName.AspNetCore",
        "src/CompanyName.AspNetCore.Mvc",
        "src/CompanyName.Auditing",
        "src/CompanyName.Auditing.Abstracts",
        "src/CompanyName.Autofac",
        "src/CompanyName.AutoMapper",
        "src/CompanyName.Caching",
        "src/CompanyName.Caching.StackExchangeRedis",
        "src/CompanyName.Core",
        "src/CompanyName.Dapper",
        "src/CompanyName.Dapper.SqlServer",
        "src/CompanyName.Data",
        "src/CompanyName.Ddd.Application",
        "src/CompanyName.Ddd.Domain",
        "src/CompanyName.Ddd.Domain.EventBus",
        "src/CompanyName.EntityFrameworkCore",
        "src/CompanyName.EntityFrameworkCore.Abstracts",
        "src/CompanyName.EntityFrameworkCore.SqlServer",
        "src/CompanyName.EventBus",
        "src/CompanyName.EventBus.Abstracts",
        "src/CompanyName.EventBus.Azure",
        "src/CompanyName.Security",
        "src/CompanyName.ServiceBus.Abstracts",
        "src/CompanyName.ServiceBus.Azure",
        "src/CompanyName.ServiceBus.Local",
        "src/CompanyName.TestBase",
        "src/CompanyName.Uow"
    )
  }

$templates = [PSCustomObject]@{
    BasePath = "templates"
    Solution = $null
    Projects = (
        "src/CompanyName.Templates"
    )
  }


# List of solutions
$solutions = (
    $framework,
    $templates
)