#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WebApi/WebAPI/WebAPI.csproj", "WebApi/WebAPI/"]
COPY ["WebApi/Application/Application.csproj", "WebApi/Application/"]
COPY ["WebApi/Domain/Domain.csproj", "WebApi/Domain/"]
COPY ["WebApi/Persistence/Persistence.csproj", "WebApi/Persistence/"]
RUN dotnet restore "WebApi/WebAPI/WebAPI.csproj"
COPY . .
WORKDIR "/src/WebApi/WebAPI"
RUN dotnet build "WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RetroLauncher.WebAPI.dll"]