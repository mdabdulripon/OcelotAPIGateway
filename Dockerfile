# Use .NET SDK image for building (ARM64)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files individually to improve caching
COPY ["OcelotAPIGateway/OcelotAPIGateway.csproj", "./"]
RUN dotnet restore "OcelotAPIGateway.csproj"

# Copy the entire project for building
COPY . .

# Set the working directory to Merchant.API and build the project
WORKDIR /src/
RUN dotnet build "OcelotAPIGateway/OcelotAPIGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish project
FROM build AS publish
RUN dotnet publish "OcelotAPIGateway/OcelotAPIGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use ASP.NET runtime image for deployment (ARM64)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Expose port (optional, adjust if necessary)
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "OcelotAPIGateway.dll"]
