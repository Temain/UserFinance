FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["UserFinance.sln", "./"]
COPY ["global.json", "./"]
COPY ["src/Common/UserFinance.Common/UserFinance.Common.csproj", "src/Common/UserFinance.Common/"]
COPY ["src/UserService/UserService.Abstractions/UserService.Abstractions.csproj", "src/UserService/UserService.Abstractions/"]
COPY ["src/UserService/UserService.Domain/UserService.Domain.csproj", "src/UserService/UserService.Domain/"]
COPY ["src/UserService/UserService.Infrastructure/UserService.Infrastructure.csproj", "src/UserService/UserService.Infrastructure/"]
COPY ["src/UserService/UserService.Migrations/UserService.Migrations.csproj", "src/UserService/UserService.Migrations/"]
COPY ["src/FinanceService/FinanceService.Abstractions/FinanceService.Abstractions.csproj", "src/FinanceService/FinanceService.Abstractions/"]
COPY ["src/FinanceService/FinanceService.Domain/FinanceService.Domain.csproj", "src/FinanceService/FinanceService.Domain/"]
COPY ["src/FinanceService/FinanceService.Infrastructure/FinanceService.Infrastructure.csproj", "src/FinanceService/FinanceService.Infrastructure/"]
COPY ["src/FinanceService/FinanceService.Migrations/FinanceService.Migrations.csproj", "src/FinanceService/FinanceService.Migrations/"]
COPY ["src/MigrationService/MigrationService/MigrationService.csproj", "src/MigrationService/MigrationService/"]

RUN dotnet restore "src/MigrationService/MigrationService/MigrationService.csproj"

COPY . .
RUN dotnet publish "src/MigrationService/MigrationService/MigrationService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MigrationService.dll"]
