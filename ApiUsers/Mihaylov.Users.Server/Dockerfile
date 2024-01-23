#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiUsers/Mihaylov.Users.Server/Mihaylov.Users.Server.csproj", "ApiUsers/Mihaylov.Users.Server/"]
COPY ["ApiUsers/Mihaylov.Users.Controllers/Mihaylov.Users.Controllers.csproj", "ApiUsers/Mihaylov.Users.Controllers/"]
COPY ["Common/Mihaylov.Common.Host/Mihaylov.Common.Host.csproj", "Common/Mihaylov.Common.Host/"]
COPY ["ApiUsers/Mihaylov.Users.Data/Mihaylov.Users.Data.csproj", "ApiUsers/Mihaylov.Users.Data/"]
COPY ["Common/Mihaylov.Common.Abstract/Mihaylov.Common.Abstract.csproj", "Common/Mihaylov.Common.Abstract/"]
COPY ["ApiUsers/Mihaylov.Users.Models/Mihaylov.Users.Models.csproj", "ApiUsers/Mihaylov.Users.Models/"]
RUN dotnet restore "ApiUsers/Mihaylov.Users.Server/Mihaylov.Users.Server.csproj"
COPY . .
WORKDIR "/src/ApiUsers/Mihaylov.Users.Server"
RUN dotnet build "Mihaylov.Users.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mihaylov.Users.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mihaylov.Users.Server.dll"]