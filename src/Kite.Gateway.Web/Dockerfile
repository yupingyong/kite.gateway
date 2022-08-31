#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Kite.Gateway.Web/Kite.Gateway.Web.csproj", "src/Kite.Gateway.Web/"]
COPY ["src/Kite.Gateway.Application/Kite.Gateway.Application.csproj", "src/Kite.Gateway.Application/"]
COPY ["src/Kite.Gateway.Application.Contracts/Kite.Gateway.Application.Contracts.csproj", "src/Kite.Gateway.Application.Contracts/"]
COPY ["src/Kite.Gateway.Domain.Shared/Kite.Gateway.Domain.Shared.csproj", "src/Kite.Gateway.Domain.Shared/"]
COPY ["src/Kite.Gateway.Domain/Kite.Gateway.Domain.csproj", "src/Kite.Gateway.Domain/"]
RUN dotnet restore "src/Kite.Gateway.Web/Kite.Gateway.Web.csproj"
COPY . .
WORKDIR "/src/src/Kite.Gateway.Web"
RUN dotnet build "Kite.Gateway.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kite.Gateway.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kite.Gateway.Web.dll"]