FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["UserFinance.sln", "./"]
COPY ["global.json", "./"]
COPY ["src/Shared/UserFinance.Common/UserFinance.Common.csproj", "src/Shared/UserFinance.Common/"]
COPY ["src/UserService/UserService.Abstractions/UserService.Abstractions.csproj", "src/UserService/UserService.Abstractions/"]
COPY ["src/UserService/UserService.Domain/UserService.Domain.csproj", "src/UserService/UserService.Domain/"]
COPY ["src/UserService/UserService.Business/UserService.Business.csproj", "src/UserService/UserService.Business/"]
COPY ["src/UserService/UserService.Application/UserService.Application.csproj", "src/UserService/UserService.Application/"]
COPY ["src/UserService/UserService.Infrastructure/UserService.Infrastructure.csproj", "src/UserService/UserService.Infrastructure/"]
COPY ["src/UserService/UserService.Api/UserService.Api.csproj", "src/UserService/UserService.Api/"]

RUN dotnet restore "src/UserService/UserService.Api/UserService.Api.csproj"

COPY . .
RUN dotnet publish "src/UserService/UserService.Api/UserService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UserService.Api.dll"]
