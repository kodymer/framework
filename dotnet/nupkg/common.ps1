# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

$framework = [PSCustomObject]@{
    BasePath = "framework"
    Solution = "CompanyName.Framework"
    Projects = (
       "src/CompanyName.AspNetCore",
       "src/CompanyName.AspNetCore.Abstractions",
       "src/CompanyName.AspNetCore.Mvc",
       "src/CompanyName.Auditing",
       "src/CompanyName.Auditing.Abstractions",
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
       "src/CompanyName.Ddd.Domain.Abstractions",
       "src/CompanyName.Ddd.Domain.EventBus",
       "src/CompanyName.EntityFrameworkCore",
       "src/CompanyName.EntityFrameworkCore.Abstractions",
       "src/CompanyName.EntityFrameworkCore.SqlServer",
       "src/CompanyName.EventBus",
       "src/CompanyName.EventBus.Abstractions",
       "src/CompanyName.EventBus.AzureServiceBus",
       "src/CompanyName.Localization",
       "src/CompanyName.Messaging.Abstractions",
       "src/CompanyName.Messaging.AzureServiceBus",
       "src/CompanyName.Messaging.InProcess",
       "src/CompanyName.Security",
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

$modules = [PSCustomObject]@{
    BasePath = "modules"
    Solution = $null
    Projects = (
    )
}