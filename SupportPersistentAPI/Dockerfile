FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SupportPersistentAPI/SupportPersistentAPI.csproj", "SupportPersistentAPI/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "./SupportPersistentAPI/SupportPersistentAPI.csproj"
COPY . .
WORKDIR "/src/SupportPersistentAPI"
RUN dotnet build "./SupportPersistentAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SupportPersistentAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SupportPersistentAPI.dll"]