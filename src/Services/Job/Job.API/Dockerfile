#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY ["src/Common/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "src/Common/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj"]
COPY ["src/Common/Jobs.Service.Common/Jobs.Service.Common.csproj", "src/Common/Jobs.Service.Common/Jobs.Service.Common.csproj"]
COPY ["src/Services/Job/Job.API/Job.API.csproj", "src/Services/Job/Job.API/Job.API.csproj"]
RUN dotnet restore "src/Services/Job/Job.API/Job.API.csproj"

COPY . .
WORKDIR "/source/src/Services/Job/Job.API"
RUN dotnet build "Job.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Job.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Job.API.dll"]