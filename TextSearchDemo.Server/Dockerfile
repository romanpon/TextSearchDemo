#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TextSearchDemo.Server/TextSearchDemo.Server.csproj", "TextSearchDemo.Server/"]
COPY ["TextSearchDemo.Trie/TextSearchDemo.Trie.csproj", "TextSearchDemo.Trie/"]
RUN dotnet restore "./TextSearchDemo.Server/./TextSearchDemo.Server.csproj"
COPY . .
WORKDIR "/src/TextSearchDemo.Server"
RUN dotnet build "./TextSearchDemo.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TextSearchDemo.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TextSearchDemo.Server.dll"]