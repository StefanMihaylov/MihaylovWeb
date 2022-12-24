#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Site/Mihaylov.Site.Server/Mihaylov.Site.Server.csproj", "Site/Mihaylov.Site.Server/"]
COPY ["Site/Mihaylov.Site.Core/Mihaylov.Site.Core.csproj", "Site/Mihaylov.Site.Core/"]
COPY ["Site/Mihaylov.Site.Core.Contracts/Mihaylov.Site.Core.Contracts.csproj", "Site/Mihaylov.Site.Core.Contracts/"]
COPY ["Site/Mihaylov.Site.Data.Contracts/Mihaylov.Site.Data.Contracts.csproj", "Site/Mihaylov.Site.Data.Contracts/"]
COPY ["Site/Mihaylov.Site.Database/Mihaylov.Site.Database.csproj", "Site/Mihaylov.Site.Database/"]
COPY ["Common/Mihaylov.Common.Abstract/Mihaylov.Common.Abstract.csproj", "Common/Mihaylov.Common.Abstract/"]
COPY ["Site/Mihaylov.Site.Data/Mihaylov.Site.Data.csproj", "Site/Mihaylov.Site.Data/"]
RUN dotnet restore "Site/Mihaylov.Site.Server/Mihaylov.Site.Server.csproj"
COPY . .
WORKDIR "/src/Site/Mihaylov.Site.Server"
RUN dotnet build "Mihaylov.Site.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mihaylov.Site.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mihaylov.Site.Server.dll"]