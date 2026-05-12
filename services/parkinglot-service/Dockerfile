FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore ./src/ParkingLot.API/ParkingLot.API.csproj
RUN dotnet publish ./src/ParkingLot.API/ParkingLot.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5002

ENTRYPOINT ["dotnet", "ParkingLot.API.dll"]