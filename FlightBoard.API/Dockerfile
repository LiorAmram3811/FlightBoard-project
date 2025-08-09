# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files first (only project files) to leverage Docker layer caching
COPY FlightBoard.sln ./
COPY FlightBoard.Domain/FlightBoard.Domain.csproj FlightBoard.Domain/
COPY FlightBoard.Application/FlightBoard.Application.csproj FlightBoard.Application/
COPY FlightBoard.Infrastructure/FlightBoard.Infrastructure.csproj FlightBoard.Infrastructure/
COPY FlightBoard.API/FlightBoard.API.csproj FlightBoard.API/
COPY FlightBoard.Tests/FlightBoard.Tests.csproj FlightBoard.Tests/

# Restore explicitly against the API project
RUN dotnet restore FlightBoard.API/FlightBoard.API.csproj

# Now copy the rest of the source
COPY . .

# Publish
WORKDIR /src/FlightBoard.API
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FlightBoard.API.dll"]
