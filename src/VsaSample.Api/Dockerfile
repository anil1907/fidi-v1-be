# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/VsaSample.Api/VsaSample.Api.csproj", "VsaSample.Api/"]
COPY ["src/VsaSample.Application/VsaSample.Application.csproj", "VsaSample.Application/"]
COPY ["src/VsaSample.Infrastructure/VsaSample.Infrastructure.csproj", "VsaSample.Infrastructure/"]
COPY ["src/VsaSample.Domain/VsaSample.Domain.csproj", "VsaSample.Domain/"]
COPY ["src/VsaSample.SharedKernel/VsaSample.SharedKernel.csproj", "VsaSample.SharedKernel/"]
RUN dotnet restore "VsaSample.Api/VsaSample.Api.csproj"

# Copy the remaining source code and publish
COPY src/ .
WORKDIR /src/VsaSample.Api
RUN dotnet publish "VsaSample.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS="http://+:8080"
ENTRYPOINT ["dotnet", "VsaSample.Api.dll"]
