# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

# List of solutions
$solutions = (
    "framework"
)

# List of projects
$projects = (

    # framework
    "framework/src/Vesta.ApplicationInsights.AspNetCore",
    "framework/src/Vesta.AspNetCore",
    "framework/src/Vesta.AspNetCore.Mvc",
    "framework/src/Vesta.Auditing",
    "framework/src/Vesta.Auditing.Abstracts",
    "framework/src/Vesta.Autofac",
    "framework/src/Vesta.AutoMapper",
    "framework/src/Vesta.Core",
    "framework/src/Vesta.Ddd.Application",
    "framework/src/Vesta.Ddd.Domain",
    "framework/src/Vesta.Ddd.Domain.EventBus",
    "framework/src/Vesta.EntityFrameworkCore.Abstracts",
    "framework/src/Vesta.EntityFrameworkCore",
    "framework/src/Vesta.EntityFrameworkCore.SqlServer",
	"framework/src/Vesta.Dapper",
    "framework/src/Vesta.EventBus",
    "framework/src/Vesta.EventBus.Abstracts",
    "framework/src/Vesta.EventBus.Azure",
    "framework/src/Vesta.ServiceBus.Azure",
    "framework/src/Vesta.Security",
    "framework/src/Vesta.Uow"
)
