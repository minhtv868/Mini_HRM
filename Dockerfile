# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY WebJob/*.csproj ./WebJob/
COPY Web.Application/*.csproj ./Web.Application/
COPY Web.Domain/*.csproj ./Web.Domain/
COPY Web.Infrastructure/*.csproj ./Web.Infrastructure/
COPY Web.Persistence/*.csproj ./Web.Persistence/
COPY Web.Shared/*.csproj ./Web.Shared/

RUN dotnet restore --use-current-runtime  

# copy everything else and build app
COPY WebJob/. ./WebJob/
COPY Web.Application/. ./Web.Application/
COPY Web.Domain/. ./Web.Domain/
COPY Web.Infrastructure/. ./Web.Infrastructure/
COPY Web.Persistence/. ./Web.Persistence/
COPY Web.Shared/. ./Web.Shared/

WORKDIR /source/WebJob

RUN dotnet publish --use-current-runtime --self-contained false --no-restore -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV TZ="Asia/Bangkok"
WORKDIR /app
COPY --from=build /app ./
COPY --from=build /app/appsettings.dev.json ./appsettings.json
EXPOSE 8080
ENTRYPOINT ["dotnet", "WebJob.dll"]