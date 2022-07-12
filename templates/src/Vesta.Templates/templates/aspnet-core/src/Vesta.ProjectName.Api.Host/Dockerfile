FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Api.Host/Api.Host.csproj", "src/Api.Host/"]
COPY ["src/Api/Api.csproj", "src/Api/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infraestructure/Infraestructure.csproj", "src/Infraestructure/"]
RUN dotnet restore "src/Api.Host/Api.Host.csproj"
COPY . .
WORKDIR "/src/src/Api.Host"
RUN dotnet build "Api.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.Host.dll"]