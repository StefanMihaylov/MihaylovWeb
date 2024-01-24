#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiWeather/Mihaylov.Api.Weather/Mihaylov.Api.Weather.csproj", "ApiWeather/Mihaylov.Api.Weather/"]
COPY ["Common/Mihaylov.Common.Host/Mihaylov.Common.Host.csproj", "Common/Mihaylov.Common.Host/"]
COPY ["ApiWeather/Mihaylov.Api.Weather.Data/Mihaylov.Api.Weather.Data.csproj", "ApiWeather/Mihaylov.Api.Weather.Data/"]
COPY ["ApiWeather/Mihaylov.Api.Weather.Contracts/Mihaylov.Api.Weather.Contracts.csproj", "ApiWeather/Mihaylov.Api.Weather.Contracts/"]
RUN dotnet restore "ApiWeather/Mihaylov.Api.Weather/Mihaylov.Api.Weather.csproj"
COPY . .
WORKDIR "/src/ApiWeather/Mihaylov.Api.Weather"
RUN dotnet build "Mihaylov.Api.Weather.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mihaylov.Api.Weather.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mihaylov.Api.Weather.dll"]