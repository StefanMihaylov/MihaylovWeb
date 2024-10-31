# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiDictionary/Mihaylov.Api.Dictionary/Mihaylov.Api.Dictionary.csproj", "ApiDictionary/Mihaylov.Api.Dictionary/"]
COPY ["Common/Mihaylov.Common.Host/Mihaylov.Common.Host.csproj", "Common/Mihaylov.Common.Host/"]
COPY ["Common/Mihaylov.Common.Host.Abstract/Mihaylov.Common.Host.Abstract.csproj", "Common/Mihaylov.Common.Host.Abstract/"]
COPY ["ApiDictionary/Mihaylov.Api.Dictionary.DAL/Mihaylov.Api.Dictionary.DAL.csproj", "ApiDictionary/Mihaylov.Api.Dictionary.DAL/"]
COPY ["ApiDictionary/Mihaylov.Api.Dictionary.Database/Mihaylov.Api.Dictionary.Database.csproj", "ApiDictionary/Mihaylov.Api.Dictionary.Database/"]
COPY ["Common/Mihaylov.Common.Database/Mihaylov.Common.Database.csproj", "Common/Mihaylov.Common.Database/"]
COPY ["ApiDictionary/Mihaylov.Api.Dictionary.Contracts/Mihaylov.Api.Dictionary.Contracts.csproj", "ApiDictionary/Mihaylov.Api.Dictionary.Contracts/"]
COPY ["ApiDictionary/Mihaylov.Api.Dictionary.Data/Mihaylov.Api.Dictionary.Data.csproj", "ApiDictionary/Mihaylov.Api.Dictionary.Data/"]
RUN dotnet restore "./ApiDictionary/Mihaylov.Api.Dictionary/Mihaylov.Api.Dictionary.csproj"
COPY . .
WORKDIR "/src/ApiDictionary/Mihaylov.Api.Dictionary"
RUN dotnet build "./Mihaylov.Api.Dictionary.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mihaylov.Api.Dictionary.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mihaylov.Api.Dictionary.dll"]