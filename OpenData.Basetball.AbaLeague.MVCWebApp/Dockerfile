#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OpenData.Basetball.AbaLeague.MVCWebApp/OpenData.Basetball.AbaLeague.MVCWebApp.csproj", "OpenData.Basetball.AbaLeague.MVCWebApp/"]
COPY ["OpenData.Basetball.AbaLeague.Application/OpenData.Basketball.AbaLeague.Application.csproj", "OpenData.Basetball.AbaLeague.Application/"]
COPY ["OpenData.Basetball.AbaLeague.Crawler/OpenData.Basetball.AbaLeague.Crawler.csproj", "OpenData.Basetball.AbaLeague.Crawler/"]
COPY ["OpenData.Basetball.AbaLeague.Domain/OpenData.Basketball.AbaLeague.Domain.csproj", "OpenData.Basetball.AbaLeague.Domain/"]
COPY ["OpenData.Basetball.AbaLeague.Persistence/OpenData.Basketball.AbaLeague.Persistence.csproj", "OpenData.Basetball.AbaLeague.Persistence/"]
RUN dotnet restore "./OpenData.Basetball.AbaLeague.MVCWebApp/./OpenData.Basetball.AbaLeague.MVCWebApp.csproj"
COPY . .
WORKDIR "/src/OpenData.Basetball.AbaLeague.MVCWebApp"
RUN dotnet build "./OpenData.Basetball.AbaLeague.MVCWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OpenData.Basetball.AbaLeague.MVCWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenData.Basetball.AbaLeague.MVCWebApp.dll"]