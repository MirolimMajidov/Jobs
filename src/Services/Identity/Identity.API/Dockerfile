#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 81

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY ["src/Common/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "src/Common/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj"]
COPY ["src/Common/Jobs.Service.Common/Jobs.Service.Common.csproj", "src/Common/Jobs.Service.Common/Jobs.Service.Common.csproj"]
COPY ["src/Services/Identity/Identity.API/Identity.API.csproj", "src/Services/Identity/Identity.API/Identity.API.csproj"]
RUN dotnet restore "src/Services/Identity/Identity.API/Identity.API.csproj"

COPY . .
WORKDIR "/source/src/Services/Identity/Identity.API"
RUN dotnet build "Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.dll"]