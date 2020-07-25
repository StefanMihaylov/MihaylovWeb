#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Users/Mihaylov.Users.Server/Mihaylov.Users.Server.csproj", "Users/Mihaylov.Users.Server/"]
COPY ["Users/Mihaylov.Users.Data/Mihaylov.Users.Data.csproj", "Users/Mihaylov.Users.Data/"]
RUN dotnet restore "Users/Mihaylov.Users.Server/Mihaylov.Users.Server.csproj"
COPY . .
WORKDIR "/src/Users/Mihaylov.Users.Server"
RUN dotnet build "Mihaylov.Users.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mihaylov.Users.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mihaylov.Users.Server.dll"]