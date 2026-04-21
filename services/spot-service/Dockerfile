FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore src/Spot.API/Spot.API.csproj
RUN dotnet publish src/Spot.API/Spot.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5003

EXPOSE 5003

ENTRYPOINT ["dotnet", "Spot.API.dll"]