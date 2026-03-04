# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["AdvertisingApp/AdvertisingApp.sln", "./"]
COPY ["AdvertisingApp/AdvertisingApp.Web/AdvertisingApp.Web.csproj", "AdvertisingApp/AdvertisingApp.Web/"]
COPY ["AdvertisingApp/AdvertisingApp.Core/AdvertisingApp.Core.csproj", "AdvertisingApp/AdvertisingApp.Core/"]
COPY ["AdvertisingApp/AdvertisingApp.Infrastructure/AdvertisingApp.Infrastructure.csproj", "AdvertisingApp/AdvertisingApp.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "AdvertisingApp/AdvertisingApp.Web/AdvertisingApp.Web.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/AdvertisingApp/AdvertisingApp.Web"
RUN dotnet build "AdvertisingApp.Web.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "AdvertisingApp.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Install libgdiplus for System.Drawing (required by some packages)
RUN apt-get update && apt-get install -y --no-install-recommends \
    libgdiplus \
    && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

# Expose port
EXPOSE 5000
EXPOSE 5001

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "AdvertisingApp.Web.dll"]
