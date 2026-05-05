FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["UserFinance.sln", "./"]
COPY ["global.json", "./"]
COPY ["src/Common/UserFinance.Common/UserFinance.Common.csproj", "src/Common/UserFinance.Common/"]
COPY ["src/CurrencyUpdater/CurrencyUpdater.Application/CurrencyUpdater.Application.csproj", "src/CurrencyUpdater/CurrencyUpdater.Application/"]
COPY ["src/CurrencyUpdater/CurrencyUpdater.Infrastructure/CurrencyUpdater.Infrastructure.csproj", "src/CurrencyUpdater/CurrencyUpdater.Infrastructure/"]
COPY ["src/CurrencyUpdater/CurrencyUpdater.Worker/CurrencyUpdater.Worker.csproj", "src/CurrencyUpdater/CurrencyUpdater.Worker/"]

RUN dotnet restore "src/CurrencyUpdater/CurrencyUpdater.Worker/CurrencyUpdater.Worker.csproj"

COPY . .
RUN dotnet publish "src/CurrencyUpdater/CurrencyUpdater.Worker/CurrencyUpdater.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CurrencyUpdater.Worker.dll"]
