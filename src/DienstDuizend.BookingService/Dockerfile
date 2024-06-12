FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DienstDuizend.BookingService/DienstDuizend.BookingService.csproj", "src/DienstDuizend.BookingService/"]
RUN dotnet restore "src/DienstDuizend.BookingService/DienstDuizend.BookingService.csproj"
COPY . .
WORKDIR "/src/src/DienstDuizend.BookingService"
RUN dotnet build "DienstDuizend.BookingService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DienstDuizend.BookingService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DienstDuizend.BookingService.dll"]
