FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["UserFinance.sln", "./"]
COPY ["global.json", "./"]
COPY ["src/Shared/UserFinance.Common/UserFinance.Common.csproj", "src/Shared/UserFinance.Common/"]
COPY ["src/ApiGateway/ApiGateway/ApiGateway.csproj", "src/ApiGateway/ApiGateway/"]

RUN dotnet restore "src/ApiGateway/ApiGateway/ApiGateway.csproj"

COPY . .
RUN dotnet publish "src/ApiGateway/ApiGateway/ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
