FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/CompanyName.Banks.EntityFrameworkCore.SqlServer/CompanyName.Banks.EntityFrameworkCore.SqlServer.csproj", "src/CompanyName.Banks.EntityFrameworkCore.SqlServer/"]
COPY ["src/CompanyName.Banks.Dapper.SqlServer/CompanyName.Banks.Dapper.SqlServer.csproj", "src/CompanyName.Banks.Dapper.SqlServer/"]
COPY ["src/CompanyName.Banks.Domain/CompanyName.Banks.Domain.csproj", "src/CompanyName.Banks.Domain/"]
COPY ["src/CompanyName.Banks.Domain/CompanyName.Banks.Domain.Shared.csproj", "src/CompanyName.Banks.Domain.Shared/"]
COPY ["src/CompanyName.Banks.Application/CompanyName.Banks.Application.csproj", "src/CompanyName.Banks.Application/"]
COPY ["src/CompanyName.Banks.Api/CompanyName.Banks.Api.csproj", "src/CompanyName.Banks.Api/"]
COPY ["src/CompanyName.Banks.Api.Host/CompanyName.Banks.Api.Host.csproj", "src/CompanyName.Banks.Api.Host/"]
RUN dotnet restore "src/CompanyName.Banks.Api.Host/CompanyName.Banks.Api.Host.csproj"
COPY . .
WORKDIR "/src/src/CompanyName.Banks.Api.Host"
RUN dotnet build "CompanyName.Banks.Api.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CompanyName.Banks.Api.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CompanyName.Banks.Api.Host.dll"]