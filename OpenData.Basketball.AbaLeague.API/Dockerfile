#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OpenData.Basketball.AbaLeague.API/OpenData.Basketball.AbaLeague.API.csproj", "OpenData.Basketball.AbaLeague.API/"]
RUN dotnet restore "./OpenData.Basketball.AbaLeague.API/OpenData.Basketball.AbaLeague.API.csproj"
COPY . .
WORKDIR "/src/OpenData.Basketball.AbaLeague.API"
RUN dotnet build "./OpenData.Basketball.AbaLeague.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OpenData.Basketball.AbaLeague.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenData.Basketball.AbaLeague.API.dll"]