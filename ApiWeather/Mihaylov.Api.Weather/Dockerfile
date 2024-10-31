# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiWeather/Mihaylov.Api.Weather/Mihaylov.Api.Weather.csproj", "ApiWeather/Mihaylov.Api.Weather/"]
COPY ["Common/Mihaylov.Common.Host/Mihaylov.Common.Host.csproj", "Common/Mihaylov.Common.Host/"]
COPY ["Common/Mihaylov.Common.Host.Abstract/Mihaylov.Common.Host.Abstract.csproj", "Common/Mihaylov.Common.Host.Abstract/"]
COPY ["ApiWeather/Mihaylov.Api.Weather.Data/Mihaylov.Api.Weather.Data.csproj", "ApiWeather/Mihaylov.Api.Weather.Data/"]
COPY ["ApiWeather/Mihaylov.Api.Weather.Contracts/Mihaylov.Api.Weather.Contracts.csproj", "ApiWeather/Mihaylov.Api.Weather.Contracts/"]
RUN dotnet restore "./ApiWeather/Mihaylov.Api.Weather/Mihaylov.Api.Weather.csproj"
COPY . .
WORKDIR "/src/ApiWeather/Mihaylov.Api.Weather"
RUN dotnet build "./Mihaylov.Api.Weather.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mihaylov.Api.Weather.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mihaylov.Api.Weather.dll"]