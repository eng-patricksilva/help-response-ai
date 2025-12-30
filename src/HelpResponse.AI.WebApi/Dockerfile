# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HelpResponse.AI.WebApi/HelpResponse.AI.WebApi.csproj", "src/HelpResponse.AI.WebApi/"]
COPY ["src/HelpResponse.AI.Domain/HelpResponse.AI.Domain.csproj", "src/HelpResponse.AI.Domain/"]
COPY ["src/HelpResponse.AI.Infra.ClientHttp/HelpResponse.AI.Infra.ClientHttp.csproj", "src/HelpResponse.AI.Infra.ClientHttp/"]
COPY ["src/HelpResponse.AI.Services/HelpResponse.AI.Services.csproj", "src/HelpResponse.AI.Services/"]
RUN dotnet restore "./src/HelpResponse.AI.WebApi/HelpResponse.AI.WebApi.csproj"
COPY . .
WORKDIR "/src/src/HelpResponse.AI.WebApi"
RUN dotnet build "./HelpResponse.AI.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HelpResponse.AI.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelpResponse.AI.WebApi.dll"]