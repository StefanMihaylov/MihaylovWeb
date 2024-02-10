#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Web/Mihaylov.Web/Mihaylov.Web.csproj", "Web/Mihaylov.Web/"]
COPY ["Common/Mihaylov.Common.Host/Mihaylov.Common.Host.csproj", "Common/Mihaylov.Common.Host/"]
COPY ["Common/Mihaylov.Common.Host.Abstract/Mihaylov.Common.Host.Abstract.csproj", "Common/Mihaylov.Common.Host.Abstract/"]
COPY ["Site/Mihaylov.Site.Media/Mihaylov.Site.Media.csproj", "Site/Mihaylov.Site.Media/"]
COPY ["Web/Mihaylov.Web.Service/Mihaylov.Web.Service.csproj", "Web/Mihaylov.Web.Service/"]
COPY ["ApiUsers/Mihaylov.Api.Users.Client/Mihaylov.Api.Users.Client.csproj", "ApiUsers/Mihaylov.Api.Users.Client/"]
COPY ["ApiWeather/Mihaylov.Api.Weather.Client/Mihaylov.Api.Weather.Client.csproj", "ApiWeather/Mihaylov.Api.Weather.Client/"]
RUN dotnet restore "./Web/Mihaylov.Web/./Mihaylov.Web.csproj"
COPY . .
WORKDIR "/src/Web/Mihaylov.Web"
RUN dotnet build "./Mihaylov.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mihaylov.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mihaylov.Web.dll"]