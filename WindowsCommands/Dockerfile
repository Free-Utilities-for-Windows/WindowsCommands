FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WindowsCommands/WindowsCommands.csproj", "WindowsCommands/"]
RUN dotnet restore "WindowsCommands/WindowsCommands.csproj"
COPY . .
WORKDIR "/src/WindowsCommands"
RUN dotnet build "WindowsCommands.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WindowsCommands.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WindowsCommands.dll"]
