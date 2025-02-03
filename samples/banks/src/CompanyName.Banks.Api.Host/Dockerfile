FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/Vesta.Banks.EntityFrameworkCore.SqlServer/Vesta.Banks.EntityFrameworkCore.SqlServer.csproj", "src/Vesta.Banks.EntityFrameworkCore.SqlServer/"]
COPY ["src/Vesta.Banks.Dapper.SqlServer/Vesta.Banks.Dapper.SqlServer.csproj", "src/Vesta.Banks.Dapper.SqlServer/"]
COPY ["src/Vesta.Banks.Domain/Vesta.Banks.Domain.csproj", "src/Vesta.Banks.Domain/"]
COPY ["src/Vesta.Banks.Domain/Vesta.Banks.Domain.Shared.csproj", "src/Vesta.Banks.Domain.Shared/"]
COPY ["src/Vesta.Banks.Application/Vesta.Banks.Application.csproj", "src/Vesta.Banks.Application/"]
COPY ["src/Vesta.Banks.Api/Vesta.Banks.Api.csproj", "src/Vesta.Banks.Api/"]
COPY ["src/Vesta.Banks.Api.Host/Vesta.Banks.Api.Host.csproj", "src/Vesta.Banks.Api.Host/"]
RUN dotnet restore "src/Vesta.Banks.Api.Host/Vesta.Banks.Api.Host.csproj"
COPY . .
WORKDIR "/src/src/Vesta.Banks.Api.Host"
RUN dotnet build "Vesta.Banks.Api.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vesta.Banks.Api.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vesta.Banks.Api.Host.dll"]