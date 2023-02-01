FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/LinkdingService/LinkdingService.csproj", "src/LinkdingService/"]
RUN dotnet restore "src/LinkdingService/LinkdingService.csproj"
COPY . .
WORKDIR "/src/src/LinkdingService"
RUN dotnet build "LinkdingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LinkdingService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir ./data
ENTRYPOINT ["dotnet", "LinkdingService.dll"]
