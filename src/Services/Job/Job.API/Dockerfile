#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

COPY ["src/Common/Jobs.Common/Jobs.Common.csproj", "src/Common/Jobs.Common/Jobs.Common.csproj"]
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