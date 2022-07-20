FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/Vesta.ProjectName.EntityFrameworkCore.SqlServer/Vesta.ProjectName.EntityFrameworkCore.SqlServer.csproj", "src/Vesta.ProjectName.EntityFrameworkCore.SqlServer/"]
COPY ["src/Vesta.ProjectName.Domain/Vesta.ProjectName.Domain.csproj", "src/Vesta.ProjectName.Domain/"]
COPY ["src/Vesta.ProjectName.Application/Vesta.ProjectName.Application.csproj", "src/Vesta.ProjectName.Application/"]
COPY ["src/Vesta.ProjectName.Api/Vesta.ProjectName.Api.csproj", "src/Vesta.ProjectName.Api/"]
COPY ["src/Vesta.ProjectName.Api.Host/Vesta.ProjectName.Api.Host.csproj", "src/Vesta.ProjectName.Api.Host/"]
RUN dotnet restore "src/Vesta.ProjectName.Api.Host/Vesta.ProjectName.Api.Host.csproj"
COPY . .
WORKDIR "/src/src/Vesta.ProjectName.Api.Host"
RUN dotnet build "Vesta.ProjectName.Api.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vesta.ProjectName.Api.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vesta.ProjectName.Api.Host.dll"]