# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiSite/Mihaylov.Api.Site/Mihaylov.Api.Site.csproj", "ApiSite/Mihaylov.Api.Site/"]
COPY ["Common/Mihaylov.Common.Host/Mihaylov.Common.Host.csproj", "Common/Mihaylov.Common.Host/"]
COPY ["Common/Mihaylov.Common.Host.Abstract/Mihaylov.Common.Host.Abstract.csproj", "Common/Mihaylov.Common.Host.Abstract/"]
COPY ["ApiSite/Mihaylov.Api.Site.DAL/Mihaylov.Api.Site.DAL.csproj", "ApiSite/Mihaylov.Api.Site.DAL/"]
COPY ["Common/Mihaylov.Common.Generic/Mihaylov.Common.Generic.csproj", "Common/Mihaylov.Common.Generic/"]
COPY ["ApiSite/Mihaylov.Api.Site.Database/Mihaylov.Api.Site.Database.csproj", "ApiSite/Mihaylov.Api.Site.Database/"]
COPY ["Common/Mihaylov.Common.Database/Mihaylov.Common.Database.csproj", "Common/Mihaylov.Common.Database/"]
COPY ["ApiSite/Mihaylov.Api.Site.Contracts/Mihaylov.Api.Site.Contracts.csproj", "ApiSite/Mihaylov.Api.Site.Contracts/"]
COPY ["ApiSite/Mihaylov.Api.Site.Data/Mihaylov.Api.Site.Data.csproj", "ApiSite/Mihaylov.Api.Site.Data/"]
RUN dotnet restore "./ApiSite/Mihaylov.Api.Site/Mihaylov.Api.Site.csproj"
COPY . .
WORKDIR "/src/ApiSite/Mihaylov.Api.Site"
RUN dotnet build "./Mihaylov.Api.Site.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mihaylov.Api.Site.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mihaylov.Api.Site.dll"]