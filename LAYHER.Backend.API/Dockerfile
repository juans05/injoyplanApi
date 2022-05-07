#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["LAYHER.Backend.API/LAYHER.Backend.API.csproj", "LAYHER.Backend.API/"]
COPY ["LAYHER.Backend.Application/LAYHER.Backend.Application.csproj", "LAYHER.Backend.Application/"]
COPY ["LAYHER.Backend.Shared/LAYHER.Backend.Shared.csproj", "LAYHER.Backend.Shared/"]
COPY ["LAYHER.Backend.Domain/LAYHER.Backend.Domain.csproj", "LAYHER.Backend.Domain/"]
COPY ["Dapper.Mapper/Dapper.Mapper.csproj", "Dapper.Mapper/"]
COPY ["LAYHER.Backend.Infraestructure/LAYHER.Backend.Infraestructure.csproj", "LAYHER.Backend.Infraestructure/"]
RUN dotnet restore "LAYHER.Backend.API/LAYHER.Backend.API.csproj"
COPY . .
WORKDIR "/src/LAYHER.Backend.API"
RUN dotnet build "LAYHER.Backend.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LAYHER.Backend.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LAYHER.Backend.API.dll"]