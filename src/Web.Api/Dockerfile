#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update -yq \
    && apt-get install expect -yq
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_12.x | bash - \
    && apt-get install nodejs -yq
WORKDIR /src
COPY ["Web.Api/Web.Api.csproj", "Web.Api/"]
COPY ["Tests/Web.Api.Core.UnitTests/Web.Api.Core.UnitTests.csproj", "Tests/Web.Api.Core.UnitTests/"]
COPY ["Web.Api.Core/Web.Api.Core.csproj", "Web.Api.Core/"]
COPY ["Web.Api.Common/Web.Api.Common.csproj", "Web.Api.Common/"]
COPY ["Web.Api.Infrastructure/Web.Api.Infrastructure.csproj", "Web.Api.Infrastructure/"]
RUN dotnet restore "Web.Api/Web.Api.csproj"
COPY . .
WORKDIR "/src/Web.Api"
#RUN dotnet build "Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.Api.dll"]
