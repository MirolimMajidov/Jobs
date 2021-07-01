#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source
COPY "Jobs.sln" "Jobs.sln"

COPY ["src/Common/Jobs.Common/Jobs.Common.csproj", "src/Common/Jobs.Common/Jobs.Common.csproj"]
COPY ["src/Common/Jobs.Service.Common/Jobs.Service.Common.csproj", "src/Common/Jobs.Service.Common/Jobs.Service.Common.csproj"]
COPY ["src/Services/Payment/Payment.API/Payment.API.csproj", "src/Services/Payment/Payment.API/Payment.API.csproj"]
RUN dotnet restore "src/Services/Payment/Payment.API/Payment.API.csproj"

COPY . .
WORKDIR "/source/src/Services/Payment/Payment.API"
RUN dotnet build "Payment.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payment.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.API.dll"]