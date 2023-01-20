FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MinesweeperGame.App/MinesweeperGame.App.csproj", "MinesweeperGame.App/"]
RUN dotnet restore "MinesweeperGame.App/MinesweeperGame.App.csproj"
COPY . .
WORKDIR "/src/MinesweeperGame.App"
RUN dotnet build "MinesweeperGame.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinesweeperGame.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD dotnet MinesweeperGame.App.dll