FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["UserFinance.sln", "./"]
COPY ["global.json", "./"]
COPY ["src/Shared/UserFinance.Common/UserFinance.Common.csproj", "src/Shared/UserFinance.Common/"]
COPY ["src/FinanceService/FinanceService.Abstractions/FinanceService.Abstractions.csproj", "src/FinanceService/FinanceService.Abstractions/"]
COPY ["src/FinanceService/FinanceService.Domain/FinanceService.Domain.csproj", "src/FinanceService/FinanceService.Domain/"]
COPY ["src/FinanceService/FinanceService.Business/FinanceService.Business.csproj", "src/FinanceService/FinanceService.Business/"]
COPY ["src/FinanceService/FinanceService.Application/FinanceService.Application.csproj", "src/FinanceService/FinanceService.Application/"]
COPY ["src/FinanceService/FinanceService.Infrastructure/FinanceService.Infrastructure.csproj", "src/FinanceService/FinanceService.Infrastructure/"]
COPY ["src/FinanceService/FinanceService.Api/FinanceService.Api.csproj", "src/FinanceService/FinanceService.Api/"]

RUN dotnet restore "src/FinanceService/FinanceService.Api/FinanceService.Api.csproj"

COPY . .
RUN dotnet publish "src/FinanceService/FinanceService.Api/FinanceService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FinanceService.Api.dll"]
