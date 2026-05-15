ARG tag=8.0-slim

FROM  mcr.microsoft.com/dotnet/aspnet:${tag}

WORKDIR /app

ARG buildConfiguration=Release

COPY ./bin/${buildConfiguration}/net8.0/ /app

EXPOSE 80

ENTRYPOINT ["dotnet", "CompanyName.ProjectName.HttpApi.Host.dll"]
