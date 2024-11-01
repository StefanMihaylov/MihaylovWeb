# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Web/Mihaylov.Web/Mihaylov.Web.csproj", "Web/Mihaylov.Web/"]
COPY ["ApiSite/Mihaylov.Api.Site.Data/Mihaylov.Api.Site.Data.csproj", "ApiSite/Mihaylov.Api.Site.Data/"]
COPY ["ApiSite/Mihaylov.Api.Site.Contracts/Mihaylov.Api.Site.Contracts.csproj", "ApiSite/Mihaylov.Api.Site.Contracts/"]
COPY ["Common/Mihaylov.Common.Generic/Mihaylov.Common.Generic.csproj", "Common/Mihaylov.Common.Generic/"]
COPY ["Common/Mihaylov.Common.Host/Mihaylov.Common.Host.csproj", "Common/Mihaylov.Common.Host/"]
COPY ["Web/Mihaylov.Web.Service/Mihaylov.Web.Service.csproj", "Web/Mihaylov.Web.Service/"]
COPY ["ApiWeather/Mihaylov.Api.Weather.Client/Mihaylov.Api.Weather.Client.csproj", "ApiWeather/Mihaylov.Api.Weather.Client/"]
COPY ["ApiOther/Mihaylov.Api.Other.Client/Mihaylov.Api.Other.Client.csproj", "ApiOther/Mihaylov.Api.Other.Client/"]
COPY ["ApiSite/Mihaylov.Api.Site.Client/Mihaylov.Api.Site.Client.csproj", "ApiSite/Mihaylov.Api.Site.Client/"]
COPY ["ApiUsers/Mihaylov.Api.Users.Client/Mihaylov.Api.Users.Client.csproj", "ApiUsers/Mihaylov.Api.Users.Client/"]
RUN dotnet restore "./Web/Mihaylov.Web/Mihaylov.Web.csproj"
COPY . .
WORKDIR "/src/Web/Mihaylov.Web"
RUN dotnet build "./Mihaylov.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mihaylov.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mihaylov.Web.dll"]