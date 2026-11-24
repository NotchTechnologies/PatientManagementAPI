
# Stage 1: Build application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copy project file
COPY ["PatientManagementAPI.csproj", "./"]

# Restore dependencies
RUN dotnet restore "PatientManagementAPI.csproj"

# Copy remaining source code
COPY . .

# Build and publish
RUN dotnet publish "PatientManagementAPI.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

# Application listens on port 8080
ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "PatientManagementAPI.dll"]
