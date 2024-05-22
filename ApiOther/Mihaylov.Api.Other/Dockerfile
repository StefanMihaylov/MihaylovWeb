#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiOther/Mihaylov.Api.Other/Mihaylov.Api.Other.csproj", "ApiOther/Mihaylov.Api.Other/"]
COPY ["Common/Mihaylov.Common.Host.Abstract/Mihaylov.Common.Host.Abstract.csproj", "Common/Mihaylov.Common.Host.Abstract/"]
COPY ["Common/Mihaylov.Common.Host/Mihaylov.Common.Host.csproj", "Common/Mihaylov.Common.Host/"]
COPY ["ApiOther/Mihaylov.Api.Other.Data/Mihaylov.Api.Other.Data.csproj", "ApiOther/Mihaylov.Api.Other.Data/"]
COPY ["ApiOther/Mihaylov.Api.Other.Database/Mihaylov.Api.Other.Database.csproj", "ApiOther/Mihaylov.Api.Other.Database/"]
COPY ["Common/Mihaylov.Common.Abstract/Mihaylov.Common.Abstract.csproj", "Common/Mihaylov.Common.Abstract/"]
COPY ["ApiOther/Mihaylov.Api.Other.Contracts/Mihaylov.Api.Other.Contracts.csproj", "ApiOther/Mihaylov.Api.Other.Contracts/"]
RUN dotnet restore "./ApiOther/Mihaylov.Api.Other/Mihaylov.Api.Other.csproj"
COPY . .
WORKDIR "/src/ApiOther/Mihaylov.Api.Other"
RUN dotnet build "./Mihaylov.Api.Other.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mihaylov.Api.Other.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mihaylov.Api.Other.dll"]