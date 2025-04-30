# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiUsers/Mihaylov.Users.Server/Mihaylov.Users.Server.csproj", "ApiUsers/Mihaylov.Users.Server/"]
COPY ["Common/Mihaylov.Common.Host/Mihaylov.Common.Host.csproj", "Common/Mihaylov.Common.Host/"]
COPY ["Common/Mihaylov.Common.Generic/Mihaylov.Common.Generic.csproj", "Common/Mihaylov.Common.Generic/"]
COPY ["ApiUsers/Mihaylov.Users.Data/Mihaylov.Users.Data.csproj", "ApiUsers/Mihaylov.Users.Data/"]
COPY ["Common/Mihaylov.Common.Database/Mihaylov.Common.Database.csproj", "Common/Mihaylov.Common.Database/"]
COPY ["ApiUsers/Mihaylov.Users.Models/Mihaylov.Users.Models.csproj", "ApiUsers/Mihaylov.Users.Models/"]
RUN dotnet restore "./ApiUsers/Mihaylov.Users.Server/Mihaylov.Users.Server.csproj"
COPY . .
WORKDIR "/src/ApiUsers/Mihaylov.Users.Server"
RUN dotnet build "./Mihaylov.Users.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mihaylov.Users.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mihaylov.Users.Server.dll"]