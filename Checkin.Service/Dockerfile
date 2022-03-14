FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 8000

ENV ASPNETCORE_URLS=http://*:8000

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["Checkin.Service/Checkin.Service.csproj", "Checkin.Service/"]
RUN dotnet restore "Checkin.Service/Checkin.Service.csproj"
COPY . .
WORKDIR "/src/Checkin.Service"
RUN dotnet build "Checkin.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkin.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkin.Service.dll"]
