#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["admin/Kite.Gateway.Admin/Kite.Gateway.Admin.csproj", "admin/Kite.Gateway.Admin/"]
COPY ["src/Kite.Gateway.Application/Kite.Gateway.Application.csproj", "src/Kite.Gateway.Application/"]
COPY ["src/Kite.Gateway.Application.Contracts/Kite.Gateway.Application.Contracts.csproj", "src/Kite.Gateway.Application.Contracts/"]
COPY ["src/Kite.Gateway.EntityFrameworkCore/Kite.Gateway.EntityFrameworkCore.csproj", "src/Kite.Gateway.EntityFrameworkCore/"]
COPY ["src/Kite.Gateway.Domain/Kite.Gateway.Domain.csproj", "src/Kite.Gateway.Domain/"]
COPY ["src/Kite.Gateway.Domain.Shared/Kite.Gateway.Domain.Shared.csproj", "src/Kite.Gateway.Domain.Shared/"]
COPY ["plugins/Kite.Plugin.Abstractions/Kite.Plugin.Abstractions.csproj", "plugins/Kite.Plugin.Abstractions/"]
RUN dotnet restore "admin/Kite.Gateway.Admin/Kite.Gateway.Admin.csproj"
COPY . .
WORKDIR "/src/admin/Kite.Gateway.Admin"
RUN dotnet build "Kite.Gateway.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kite.Gateway.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kite.Gateway.Admin.dll"]

RUN echo "deb https://mirrors.tuna.tsinghua.edu.cn/debian/ sid main contrib non-free" > /etc/apt/sources.list

RUN apt-get update && apt-get install -y apt-transport-https ca-certificates apt-utils libgdiplus libc6-dev && apt-get install -y libfreetype6 && apt-get install -y libfontconfig1 && apt-get install -y fontconfig