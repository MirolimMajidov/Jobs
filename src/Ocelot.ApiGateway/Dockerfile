#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY ["src/Ocelot.ApiGateway/Ocelot.ApiGateway.csproj", "src/Ocelot.ApiGateway/Ocelot.ApiGateway.csproj"]
RUN dotnet restore "src/Ocelot.ApiGateway/Ocelot.ApiGateway.csproj"

COPY . .
WORKDIR "/source/src/Ocelot.ApiGateway"
RUN dotnet build "Ocelot.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ocelot.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ocelot.ApiGateway.dll"]