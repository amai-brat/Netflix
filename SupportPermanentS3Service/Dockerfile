FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SupportPermanentS3Service/SupportPermanentS3Service.csproj", "SupportPermanentS3Service/"]
RUN dotnet restore "SupportPermanentS3Service/SupportPermanentS3Service.csproj"
COPY . .
WORKDIR "/src/SupportPermanentS3Service"
RUN dotnet build "SupportPermanentS3Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SupportPermanentS3Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SupportPermanentS3Service.dll"]
