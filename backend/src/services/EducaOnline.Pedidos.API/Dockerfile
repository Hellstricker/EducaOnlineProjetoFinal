# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Docker
WORKDIR /src
COPY ["./backend/src/services/Educaonline.Pedidos.API/Educaonline.Pedidos.API.csproj", "src/services/Educaonline.Pedidos.API/"]
COPY ["./backend/src/building_blocks/EducaOnline.Core/EducaOnline.Core.csproj", "backend/src/building_blocks/EducaOnline.Core/"]
COPY ["./backend/src/building_blocks/EducaOnline.WebAPI.Core/EducaOnline.WebAPI.Core.csproj", "backend/src/building_blocks/EducaOnline.WebAPI.Core/"]
COPY ["./backend/src/building_blocks/EducaOnline.MessageBus/EducaOnline.MessageBus.csproj", "backend/src/building_blocks/EducaOnline.MessageBus/"]
RUN dotnet restore "./src/services/Educaonline.Pedidos.API/Educaonline.Pedidos.API.csproj"
COPY . .
WORKDIR "./backend/src/services/Educaonline.Pedidos.API"
RUN dotnet build "./Educaonline.Pedidos.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Docker
RUN dotnet publish "./Educaonline.Pedidos.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
RUN mkdir -p /app/data && \
    chmod 777 /app/data
COPY --from=publish /app/publish .
RUN chown -R $APP_UID:$APP_UID /app/data || true
ENV DOTNET_ENVIRONMENT=Docker
ENTRYPOINT ["dotnet", "Educaonline.Pedidos.API.dll"]