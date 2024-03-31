#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TravelCapstone.BackEnd.API/TravelCapstone.BackEnd.API.csproj", "TravelCapstone.BackEnd.API/"]
COPY ["TravelCapstone.BackEnd.Application/TravelCapstone.BackEnd.Application.csproj", "TravelCapstone.BackEnd.Application/"]
COPY ["TravelCapstone.BackEnd.Common/TravelCapstone.BackEnd.Common.csproj", "TravelCapstone.BackEnd.Common/"]
COPY ["TravelCapstone.BackEnd.Domain/TravelCapstone.BackEnd.Domain.csproj", "TravelCapstone.BackEnd.Domain/"]
COPY ["TravelCapstone.BackEnd.Infrastructure/TravelCapstone.BackEnd.Infrastructure.csproj", "TravelCapstone.BackEnd.Infrastructure/"]
RUN dotnet restore "./TravelCapstone.BackEnd.API/TravelCapstone.BackEnd.API.csproj"
COPY . .
WORKDIR "/src/TravelCapstone.BackEnd.API"
RUN dotnet build "./TravelCapstone.BackEnd.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TravelCapstone.BackEnd.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TravelCapstone.BackEnd.API.dll"]