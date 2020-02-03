FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_12.x | bash - \
    && apt-get install nodejs -yq
WORKDIR /src
COPY ["src/Web.Api/Web.Api.csproj", "src/Web.Api/"]
COPY ["src/Web.Api.Common/Web.Api.Common.csproj", "src/Web.Api.Common/"]
COPY ["src/Web.Api.Core/Web.Api.Core.csproj", "src/Web.Api.Core/"]
COPY ["src/Web.Api.Infrastructure/Web.Api.Infrastructure.csproj", "src/Web.Api.Infrastructure/"]
RUN dotnet restore "src/Web.Api/Web.Api.csproj"
COPY . .
WORKDIR "/src/src/Web.Api"
RUN dotnet build "Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.Api.dll"]