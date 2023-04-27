#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Hotspot/Hotspot.csproj", "Hotspot/"]
COPY ["Hotspot.Model/Hotspot.Model.csproj", "Hotspot.Model/"]
COPY ["Hotspot.Services/Hotspot.Services.csproj", "Hotspot.Services/"]
COPY ["Hotspot.Tools/Hotspot.Tools.csproj", "Hotspot.Tools/"]
RUN dotnet restore "Hotspot/Hotspot.csproj"
COPY . .
WORKDIR "/src/Hotspot"
RUN dotnet build "Hotspot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hotspot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hotspot.dll"]