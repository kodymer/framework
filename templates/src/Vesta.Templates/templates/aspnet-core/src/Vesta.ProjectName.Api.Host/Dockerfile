ARG tag=6.0

FROM  mcr.microsoft.com/dotnet/aspnet:${tag}

WORKDIR /app

ARG buildConfiguration=Release

COPY ./bin/${buildConfiguration}/net6.0/ /app

EXPOSE 80

ENTRYPOINT ["dotnet", "Vesta.ProjectName.Api.Host.dll"]